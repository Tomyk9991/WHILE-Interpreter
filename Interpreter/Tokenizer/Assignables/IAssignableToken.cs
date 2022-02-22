using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter.Interpreter
{
    public interface IAssignableToken : ITreeViewElement
    {
        public uint Evaluate();

        public static IAssignableToken Parse(string assignment)
        {
            var nameAssignmentToken = new NameToken().Parse(new CodeLine(assignment));
            if (nameAssignmentToken != null)
                return nameAssignmentToken;

            var digitAssignmentToken = new DigitToken().Parse(new CodeLine(assignment));
            return digitAssignmentToken;
        }
    }
}