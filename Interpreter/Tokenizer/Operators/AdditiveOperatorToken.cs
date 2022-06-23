namespace WHILE_Interpreter.Interpreter.Operators;

public enum Operator
{
    Add,
    Sub,
    Noop
}

public class AdditiveOperatorToken : IToken, IStackable
{
    public NameToken Name { get; private set; }
    public Operator Operator { get; private set; }
    public IAssignableToken RHSOperand { get; private set; }
    
    public IToken Parse(CodeLine line)
    {
        string[] segments = line.Phrase.Split(' ', ';').Where(seg => !string.IsNullOrEmpty(seg)).ToArray();
        
            
        var nameToken = new NameToken().Parse(new CodeLine(segments[0]));
        if (nameToken == null)
            return null;

        this.Name = (NameToken) nameToken;

        this.Operator = segments[1] switch
        {
            "+=" => Operator.Add,
            "-=" => Operator.Sub,
            _ => Operator.Noop
        };

        if (Operator == Operator.Noop)
            return null;

        this.RHSOperand = IAssignableToken.Parse(string.Join("", segments[2..]));

        if (RHSOperand == null)
            return null;
        

        if (!(this as IToken).EndsWithSemicolon(line.Phrase))
        {
            InterpreterWatcher.PseudoThrow($"Expected semicolon at line: {line.Number}");
            return null;
        }

        return this;
    }

    public override string ToString()
    {
        return $"Operation: {this.Name.Value} {this.Operator.ToStringRepresentation()} {this.RHSOperand.Evaluate()}";
    }

    public List<string> ToTreeView()
        => new() { $"Operator token: {{name: {this.Name.Value}, Operator: {this.Operator.ToString() }, RHS: {this.RHSOperand.ToTreeView()[0]}}}" };
}