using System;
using WHILE_Interpreter.Interpreter.Method;

namespace WHILE_Interpreter.Interpreter
{
    public class Tokenizer
    {
        public void Tokenize(CodeLine[] lines)
        {
            // sourceCode = sourceCode.Replace("\n", "");
            TopLevelScope scope = new TopLevelScope();

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
                    MethodToken method = new MethodToken(header, lines, currentLine.Number - 1);
                    scope.Methods.Add(method);

                    if(i + 1 >= lines.Length)
                        InterpreterWatcher.PseudoThrow($"Method can't be empty at line: {i}");
                    
                    currentLine = lines[i + 1];
                    method.Parse(currentLine);
                    i = method.LatestLine();
                }
            }

            Console.WriteLine(scope.ToTreeView());
        }
    }
}