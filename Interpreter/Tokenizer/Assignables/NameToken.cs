using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WHILE_Interpreter.Interpreter
{
    public class NameToken : IAssignableToken
    {
        public string Value { get; set; }
        
        public NameToken Parse(CodeLine line)
        {
            if (Constants.KEYWORDS.Any(keyword => line.Phrase.Contains(keyword)))
                return null;

            if (!Regex.IsMatch(line.Phrase, "^[a-zA-Z_$][a-zA-Z_$0-9$]*$")) return null;

            this.Value = line.Phrase;
            return this;
        }

        public uint Evaluate()
        {
            return RunTime.GetValueFromCurrentName(this.Value);
        }


        public List<string> ToTreeView()
            => new() { $"{{Value: {this.Value}}}" };
    }
}