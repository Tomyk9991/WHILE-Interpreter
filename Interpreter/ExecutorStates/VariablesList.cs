using System.Text;
using WHILE_Interpreter.Interpreter.Operators;

namespace WHILE_Interpreter.Interpreter;

public class VariablesList
{
    private List<VariableToken> Tokens { get; } = new();
    
    public bool AddOrUpdate(VariableToken token)
    {
        if (!Tokens.Contains(token, new VariableTokenEquality()))
        {
            Tokens.Add(token);
            return true;
        }

        VariableToken t = Tokens.Find(v => v.Name.Value == token.Name.Value);
        t.Assignment = token.Assignment;

        return false;
    }
    
    public void Update(AdditiveOperatorToken operatorToken)
    {
        VariableToken t = Tokens.Find(v => v.Name.Value == operatorToken.Name.Value);

        if (t == null)
            InterpreterWatcher.PseudoThrow($"You can't use an operation on a variable, which is not defined: {operatorToken.Name.Value}");

        switch (operatorToken.Operator)
        {
            case Operator.Add:
                t.Assignment += operatorToken.RHSOperand;
                break;
            case Operator.Sub:
                t.Assignment -= operatorToken.RHSOperand;
                break;
            case Operator.Noop:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override string ToString()
    {
        return Tokens.ToMultiLineString();
    }

    private class VariableTokenEquality : IEqualityComparer<VariableToken>
    {
        public bool Equals(VariableToken x, VariableToken y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Name.Equals(y.Name) && x.Assignment.Equals(y.Assignment);
        }

        public int GetHashCode(VariableToken obj)
        {
            return HashCode.Combine(obj.Name, obj.Assignment);
        }
    }

    public VariableToken Find(Predicate<VariableToken> predicate)
    {
        return this.Tokens.Find(predicate);
    }
}