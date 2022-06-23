using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace WHILE_Interpreter.Interpreter.Method
{
    public class ReturnToken : IToken, IStackable
    {
        public IAssignableToken ReturnValue { get; private set; }
        
        private MethodHeaderToken header;

        public ReturnToken(MethodHeaderToken header)
        {
            this.header = header;
        }
        
        public IToken Parse(CodeLine line)
        {
            string[] split = line.Phrase.Split(' ', ';')
                .Where(p => !string.IsNullOrEmpty(p) && !string.IsNullOrWhiteSpace(p)).ToArray();

            if (split[0] != "return" || header == null)
                return null;

            switch (split.Length)
            {
                case 1 when header.ReturnType != TypeToken.Void:
                    InterpreterWatcher.PseudoThrow("The method is not returning the expected value)");
                    break;
                case > 2:
                    InterpreterWatcher.PseudoThrow($"Too many returning variables at line: {line.Number}");
                    return null;
            }

            if (split.Length == 2)
                ReturnValue = IAssignableToken.Parse(split[1]);

            return this;
        }

        public List<string> ToTreeView()
        {
            return this.ReturnValue == null 
                ? new List<string> { "Return token" } 
                : new List<string> { $"Return token: {this.ReturnValue.ToTreeView()[0]}" };
        }
    }
}