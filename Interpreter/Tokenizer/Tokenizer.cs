using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;
using WHILE_Interpreter.Interpreter.While;

namespace WHILE_Interpreter.Interpreter
{
    public class Tokenizer
    {
        private ILogger logger;
        public Tokenizer(ILogger logger)
        {
            this.logger = logger;
        }
        
        public TopLevelScope Tokenize(CodeLine[] lines)
        {
            TopLevelScope scope = new TopLevelScope(this.logger);

            foreach (var codeLine in lines)
            {
                logger.Log(codeLine);
            }
            
            for (int i = 0; i < lines.Length; i++)
            {
                CodeLine currentLine = lines[i];
                IToken token = scope.Parse(currentLine);
                
                if (token is IStackable stackable)
                {
                    scope.Stack.Push(stackable);
                    continue;
                }
                
                if (token is MethodHeaderToken methodHeader)
                {
                    MethodToken method = new MethodToken(methodHeader, lines, currentLine.Number);
                    
                    if(i + 1 >= lines.Length)
                        InterpreterWatcher.PseudoThrow($"Method can't be empty at line: {i}");
                    
                    currentLine = lines[i + 1];
                    MethodToken methodToken = (MethodToken) method.Parse(currentLine);

                    if (methodToken == null)
                    {
                        return scope;
                    }
                    
                    scope.Methods.Add(method);

                    if (!methodToken.EndsWithReturn)
                    {
                        InterpreterWatcher.PseudoThrow($"Method {methodHeader.ToString()} must end with return at line: {i + 1}");
                        scope.Methods.Remove(method);
                        return scope;
                    }
                    
                    i = method.LatestLine();
                }

                if (token is WhileHeaderToken whileHeaderToken)
                {
                    bool removedWhileFromStack = false;
                    WhileToken whileToken = new WhileToken(whileHeaderToken, lines);
                    
                    scope.Stack.Push(whileToken);

                    int j = 0;
                    
                    for (j = i + 1; j < lines.Length; j++)
                    {
                        currentLine = lines[j];
                        IToken innerToken = whileToken.Parse(currentLine);

                        if (innerToken is WhileToken stackedWhileToken)
                        {
                            if (!stackedWhileToken.EscapeTokenFound)
                            {
                                InterpreterWatcher.PseudoThrow("Missing escape token at line: " + whileToken.Scope.LastVisited);
                                scope.Stack.Pop();
                                removedWhileFromStack = true;
                                break;
                            }
                        }
                        
                        j = whileToken.Scope.LastVisited;

                        if (innerToken is WhileEscapeToken escapeToken)
                        {
                            whileToken.EscapeTokenFound = true;
                            break;
                        }
                    }

                    i = j;
                    if (!whileToken.EscapeTokenFound)
                    {

                        if (!removedWhileFromStack)
                        {
                            InterpreterWatcher.PseudoThrow("Missing escape token at line: " + whileToken.Scope.LastVisited);
                            scope.Stack.Pop();
                        }
                        
                        break;
                    }
                }
            }

            scope.Print();

            return scope;
        }
    }
}