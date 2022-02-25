using WHILE_Interpreter.Interpreter.Method;

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
                
                if (token is MethodHeaderToken header)
                {
                    MethodToken method = new MethodToken(header, lines, currentLine.Number);
                    scope.Methods.Add(method);

                    if(i + 1 >= lines.Length)
                        InterpreterWatcher.PseudoThrow($"Method can't be empty at line: {i}");
                    
                    currentLine = lines[i + 1];
                    method.Parse(currentLine);
                    
                    i = method.LatestLine();
                }
            }

            scope.Print();

            return scope;
        }
    }
}