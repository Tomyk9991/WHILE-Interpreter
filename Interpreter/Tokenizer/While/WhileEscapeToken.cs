using System.Linq;

namespace WHILE_Interpreter.Interpreter.While
{
    public class WhileEscapeToken : IToken
    {
        public IToken Parse(CodeLine line)
        {
            string[] split = line.Phrase.Split(' ', ';')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p) && !string.IsNullOrWhiteSpace(p)).ToArray();
            
            
            if (split[0].StartsWith("#"))
            {
                if (split[0].Length == 1 && split.Length == 1) 
                    return this;
                
                InterpreterWatcher.PseudoThrow($"Unexpected tokens after \"#\" at line: {line.Number}");
                return null;
            }

            return null;
        }

        public string ToTreeView(int indent)
        {
            return "";
        }
    }
}