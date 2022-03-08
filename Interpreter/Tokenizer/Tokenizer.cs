using WHILE_Interpreter.Interpreter.Method;
using WHILE_Interpreter.Interpreter.While;

namespace WHILE_Interpreter.Interpreter
{
    public class Tokenizer
    {
        public TopLevelScope Tokenize(CodeLine[] lines)
        {
            TopLevelScope scope = new TopLevelScope();

            foreach (var codeLine in lines)
            {
                Console.WriteLine(codeLine);
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
                    scope.Methods.Add(method);

                    if(i + 1 >= lines.Length)
                        InterpreterWatcher.PseudoThrow($"Method can't be empty at line: {i}");
                    
                    currentLine = lines[i + 1];
                    method.Parse(currentLine);
                    
                    i = method.LatestLine();
                }

                if (token is WhileHeaderToken whileHeaderToken)
                {
                    WhileToken whileToken = new WhileToken(whileHeaderToken, lines);
                    scope.Stack.Push(whileToken);

                    int j = 0;
                    
                    for (j = i + 1; j < lines.Length; j++)
                    {
                        currentLine = lines[j];
                        IToken innerToken = whileToken.Parse(currentLine);
                        j = whileToken.Scope.LastVisited;

                        if (innerToken is WhileEscapeToken escapeToken)
                            break;
                    }

                    i = j;

                    // WhileToken whileToken = new WhileToken(whileHeader, lines);
                    // scope.Stack.Push(whileToken);
                    //
                    // if (i + 1 >= lines.Length)
                    //     InterpreterWatcher.PseudoThrow($"Method can't be empty at line: {i}");
                    //
                    // currentLine = lines[i + 1];
                    // whileToken.Parse(currentLine);
                    //
                    // i = whileToken.Scope.LastVisited;
                }
            }

            scope.Print();

            return scope;
        }
    }
}