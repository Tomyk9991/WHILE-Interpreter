using System.Linq;
using System.Text;
using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter.Interpreter.While
{
    public class WhileHeaderToken : IToken, ITreeViewElement
    {
        // x != 0: 
        // The while language only can use use x != 0, so no need for real boolean algebra
        public NameToken AgainstZeroVariable { get; private set; }

        public WhileHeaderToken()
        {
            AgainstZeroVariable = null;
            
        }
        
        public WhileHeaderToken(NameToken againstZeroVariable)
        {
            AgainstZeroVariable = againstZeroVariable;
        }
        
        public IToken Parse(CodeLine line)
        {
            string[] split = line.Phrase.Split(' ', ':')
                .Where(p => !string.IsNullOrEmpty(p) && !string.IsNullOrWhiteSpace(p)).ToArray();


            if (split[0] != "while")
                return null;

            NameToken nameToken = new NameToken().Parse(new CodeLine(split[1]));

            if (nameToken == null)
            {
                InterpreterWatcher.PseudoThrow($"Expected a name at line: {line.Number}");
                return null;
            }

            this.AgainstZeroVariable = nameToken;

            if (split[2] != "!=")
            {
                InterpreterWatcher.PseudoThrow($"Expected a != at line: {line.Number}");
                return null;
            }


            if (split[3] != "0")
            {
                InterpreterWatcher.PseudoThrow($"Expected a \"0\" as comparer at line");
                return null;
            }

            if (!line.Phrase.EndsWith(":"))
            {
                InterpreterWatcher.PseudoThrow($"Expected a \":\" at the end of line: {line.Number}");
            }
            
            
            
            
            return this;
        }

        public List<string> ToTreeView()
            => new() { $"Header: {{while target: {AgainstZeroVariable.ToTreeView()[0]}}}" };
    }
}