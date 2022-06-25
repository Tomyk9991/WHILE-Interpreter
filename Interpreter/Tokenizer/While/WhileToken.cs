using System.Text;

namespace WHILE_Interpreter.Interpreter.While
{
    public class WhileToken : IToken, IStackable
    {
        public WhileHeaderToken HeaderToken { get; private set; }
        public InnerBodyScope Scope { get; set; }
        public bool EscapeTokenFound { get; set; }

        private CodeLine[] codeLines;

        public WhileToken(WhileHeaderToken headerToken, CodeLine[] codeLines)
        {
            this.HeaderToken = headerToken;
            this.codeLines = codeLines;
            this.EscapeTokenFound = false;
        }

        public IToken Parse(CodeLine line)
        {
            if (this.Scope == null)
            {
                this.Scope = new InnerBodyScope(null, codeLines);
            }
            
            return Scope.Parse(line);
        }


        public List<string> ToTreeView()
        {
            var lines = new List<string>
            {
                "├── While Token:",
                "   ├── " + this.HeaderToken.ToTreeView()[0],
                "   └── Scope:"
            };

            List<string> tempLines = this.Scope.ToTreeView();
            
            foreach (var tempLine in tempLines)
            {
                lines.Add("      " + tempLine);
            }


            return lines;
        }
    }
}