using System.Text;
using System.Text.RegularExpressions;

namespace WHILE_Interpreter.Interpreter
{
    public class DigitToken : IAssignableToken
    {
        public uint Value { get; set; }
        
        public DigitToken Parse(CodeLine line)
        {
            if (!Regex.IsMatch(line.Phrase, "^[0-9]+$")) return null;

            this.Value = (uint) int.Parse(line.Phrase);
            return this;
        }

        public uint Evaluate()
        {
            return Value;
        }

        public List<string> ToTreeView() => new() { $"{{Value: {this.Value}}}" };
    }
}