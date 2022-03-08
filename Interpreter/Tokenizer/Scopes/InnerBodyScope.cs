using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;
using WHILE_Interpreter.Interpreter.Operators;
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

            LastVisited = (int)line.Number - 1;
            
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
                
                for (int i = (int) line.Number; i < codeLines.Length; i++)
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

            AdditiveOperatorToken operatorToken = (AdditiveOperatorToken)new AdditiveOperatorToken().Parse(line);

            if (operatorToken != null)
            {
                this.Stack.Push(operatorToken);
                return operatorToken;
            }
            

            WhileEscapeToken whileEscapeToken = (WhileEscapeToken)new WhileEscapeToken().Parse(line);
            return whileEscapeToken;
        }

        public List<string> ToTreeView()
        {
            var lines = new List<string>();
            
            foreach (IStackable stackable in this.Stack)
            {
                List<string> tempLines = stackable.ToTreeView();

                for (var i = 0; i < tempLines.Count; i++)
                {
                    string tempLine = tempLines[i];
                    if (tempLines.Count == 1)
                    {
                        if (i == tempLines.Count - 1)
                            lines.Add("└─ " + tempLine);
                        else
                            lines.Add("├── " + tempLine);
                    }
                    else
                        lines.Add(tempLine);
                }
            }
            
            return lines;
        }
    }
}