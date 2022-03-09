using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;

namespace WHILE_Interpreter.Interpreter
{
    public interface IAssignableToken : ITreeViewElement
    {
        public uint Evaluate();
        
        public static IAssignableToken operator +(IAssignableToken a, IAssignableToken b)
        {
            var number = new DigitToken
            {
                Value = a.Evaluate() + b.Evaluate()
            };

            return number;
        }

        public static IAssignableToken operator -(IAssignableToken a, IAssignableToken b)
        {
            var number = new DigitToken
            {
                Value = a.Evaluate() - b.Evaluate()
            };

            return number;
        }

        public static IAssignableToken Parse(string assignment)
        {
            var nameAssignmentToken = new NameToken().Parse(new CodeLine(assignment));
            
            if (nameAssignmentToken != null)
                return nameAssignmentToken;

            var methodAssignmentToken = new MethodCallToken().Parse(new CodeLine(assignment));

            if (methodAssignmentToken != null)
                return (IAssignableToken) methodAssignmentToken;

            var digitAssignmentToken = new DigitToken().Parse(new CodeLine(assignment));
            return digitAssignmentToken;
        }
    }
}