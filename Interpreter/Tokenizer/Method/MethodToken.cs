using System.Text;

namespace WHILE_Interpreter.Interpreter.Method
{
    public class MethodToken : IToken
    {
        public MethodHeaderToken HeaderToken { get; private set; }
        public InnerBodyScope Scope { get; private set; }

        private CodeLine[] codeLines;
        private uint startIndex = 0;

        private int latestVisitedLine = 0;
        
        public MethodToken(MethodHeaderToken headerToken, CodeLine[] codeLines, uint startIndex)
        {
            this.HeaderToken = headerToken;
            
            this.codeLines = codeLines;
            this.startIndex = startIndex;
        }
        
        public int LatestLine()
        {
            return latestVisitedLine;
        }

        public IToken Parse(CodeLine line)
        {
            this.Scope = new InnerBodyScope(this.HeaderToken, codeLines);
            
            for (int i = (int) startIndex; i < codeLines.Length; i++)
            {
                CodeLine currentLine = codeLines[i];
                
                IToken token = Scope.Parse(currentLine);
                
                i = Scope.LastVisited;

                latestVisitedLine = i;
                
                if (token is ReturnToken)
                    break;
            }
            
            return this;
        }

        public string ToTreeView(int indent)
        {
            StringBuilder builder = new StringBuilder();
            
            builder.Append(' ', indent * 2).AppendLine($"Method token: {{");
            builder.AppendLine($"{this.HeaderToken.ToTreeView(indent + 1)} ");
            builder.AppendLine($"{Scope.ToTreeView(indent + 1)}");
            builder.Append(' ', indent * 2).Append("}");
            
            return builder.ToString();
        }
    }
}