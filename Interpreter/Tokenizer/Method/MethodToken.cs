using System.Text;
using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter.Interpreter.Method
{
    public class MethodToken : IToken, ITreeViewElement
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

                MethodHeaderToken headerToken = (MethodHeaderToken) new MethodHeaderToken().Parse(line);

                if (headerToken != null)
                {
                    InterpreterWatcher.PseudoThrow("Can't define a method inside a method");
                    return null;
                }

                i = Scope.LastVisited;

                latestVisitedLine = i;
                
                if (token is ReturnToken)
                    break;
            }
            
            return this;
        }

        public List<string> ToTreeView()
        {
            var lines = new List<string>
            {
                "├── Header",
                "│  ├── " + this.HeaderToken.ToTreeView()[0],
                "├── Scope:"
            };

            
            foreach (var line in this.Scope.ToTreeView())
            {
                lines.Add("   " + line);
            }

            return lines;
        }
    }
}