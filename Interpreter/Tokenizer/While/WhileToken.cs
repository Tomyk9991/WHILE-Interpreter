using System.Text;

namespace WHILE_Interpreter.Interpreter.While
{
    public class WhileToken : IToken, IStackable
    {
        public WhileHeaderToken HeaderToken { get; private set; }
        public InnerBodyScope Scope { get; private set; }
        
        private CodeLine[] codeLines;

        public WhileToken(WhileHeaderToken headerToken, CodeLine[] codeLines)
        {
            this.HeaderToken = headerToken;
            this.codeLines = codeLines;
        }

        public IToken Parse(CodeLine line)
        {
            if (this.Scope == null)
            {
                this.Scope = new InnerBodyScope(null, codeLines);
            }
            
            return Scope.Parse(line);
        }

        public string ToTreeView(int indent)
        {
            StringBuilder builder = new StringBuilder();
            
            builder.Append(' ', indent * 2).AppendLine($"While token: {{");
            builder.AppendLine($"{this.HeaderToken.ToTreeView(indent + 1)} ");
            builder.AppendLine($"{this.Scope?.ToTreeView(indent + 1)}");
            builder.Append(' ', indent * 2).Append("}");
            
            return builder.ToString();
        }
    }
}