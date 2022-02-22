using System;
using System.Linq;

namespace WHILE_Interpreter.Interpreter
{
    public class VariableToken : IToken, IStackable
    {
        public NameToken Name { get; private set; }
        public IAssignableToken Assignment { get; set; }
        
        public IToken Parse(CodeLine line)
        {
            string[] segments = line.Phrase.Split(' ', ';').Where(seg => !string.IsNullOrEmpty(seg)).ToArray();

            if (segments.Length != 3)
                return null;
            
            var nameToken = new NameToken().Parse(new CodeLine(segments[0]));
            if (nameToken == null)
                return null;

            this.Name = (NameToken) nameToken;

            if (segments[1] != "=")
                return null;

            this.Assignment = IAssignableToken.Parse(segments[2]);
            

            if (!(this as IToken).EndsWithSemicolon(line.Phrase))
            {
                InterpreterWatcher.PseudoThrow($"Expected semicolon at line: {line.Number}");
                return null;
            }

            return this;
        }

        public string ToTreeView(int indent)
        {
            string increment = new string(' ', indent * 2);
            return $"{increment}Variable token: {{name: {this.Name.Value}, Assignment: {this.Assignment.ToTreeView(0)}}}";
        }
    }
}
