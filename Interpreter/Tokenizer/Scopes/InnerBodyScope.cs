using System.Collections.Generic;
using System.Text;
using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;
using WHILE_Interpreter.Interpreter.While;

namespace WHILE_Interpreter.Interpreter
{
    public class InnerBodyScope : Scope, ITreeViewElement
    {
        public Stack<IStackable> Stack {get; set;}
        public int LastVisited { get; private set; }

        private MethodHeaderToken header;
        
        private CodeLine[] codeLines;
        private uint startIndex = 0;
        
        
        public InnerBodyScope(MethodHeaderToken header, CodeLine[] codeLines)
        {
            this.Stack = new Stack<IStackable>();
            this.header = header;
            
            this.codeLines = codeLines;
        }
        

        public override IToken Parse(CodeLine line)
        {
            VariableToken variableToken = (VariableToken) new VariableToken().Parse(line);

            LastVisited = (int)line.Number - 2;
            
            if (variableToken != null)
            {
                this.Stack.Push(variableToken);
                return variableToken;
            }

            WhileHeaderToken whileHeaderToken = (WhileHeaderToken) new WhileHeaderToken().Parse(line);
            
            if (whileHeaderToken != null)
            {
                WhileToken whileToken = new WhileToken(whileHeaderToken, codeLines);
                this.Stack.Push(whileToken);
                
                for (int i = (int) line.Number - 1; i < codeLines.Length; i++)
                {
                    CodeLine currentLine = codeLines[i];
                    IToken token = whileToken.Parse(currentLine);
                    i = whileToken.Scope.LastVisited;
                    
                    LastVisited = i;
                    
                    if (token is WhileEscapeToken escapeToken)
                        break;
                }
                

                return whileToken;
            }

            ReturnToken returnToken = (ReturnToken) new ReturnToken(header).Parse(line);

            if (returnToken != null)
            {
                this.Stack.Push(returnToken);
                return returnToken;
            }

            WhileEscapeToken whileEscapeToken = (WhileEscapeToken)new WhileEscapeToken().Parse(line);
            return whileEscapeToken;
        }

        public string ToTreeView(int indent)
        {
            StringBuilder builder = new StringBuilder();
            
            builder.Append(' ', indent * 2).AppendLine("Stack: {");
            foreach (IStackable stackable in Stack)
            {
                builder.AppendLine(stackable.ToTreeView(indent + 1));
            }

            builder.Append(' ', indent * 2).Append("}");

            return builder.ToString();
        }
    }
}