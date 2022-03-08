namespace WHILE_Interpreter.Interpreter.Method;

public class MethodCallToken : IToken, IStackable, IAssignableToken
{
    public NameToken[] Parameters { get; private set; }
    public NameToken Name { get; private set; }
    
    public IToken Parse(CodeLine line)
    {
        string[] split = line.Phrase.Split(' ', ':', '(', ')')
            .Where(p => !string.IsNullOrEmpty(p) && !string.IsNullOrWhiteSpace(p)).ToArray();


        if (!line.Phrase.Contains("(") || !line.Phrase.Contains(")"))
            return null;

        if (line.Phrase.Contains("="))
            return null;

        NameToken nameToken = new NameToken().Parse(new CodeLine(split[0]));

        if (nameToken == null)
            return null;

        this.Name = nameToken;

        NameToken[] parameters = null;
        if (split.Length > 1)
        {
            parameters = ParseParameters(split[1].Replace(",", ",#").Split("#"), line);
        }

        this.Parameters = parameters;

        return this;
    }
    
    private NameToken[] ParseParameters(string[] parameters, CodeLine line)
    {
        List<NameToken> p = new List<NameToken>();
        for (int i = 0; i < parameters.Length; i++)
        {
            // all elements except the last element must end with a comma
            bool endingWithComma = parameters[i].EndsWith(",");
                
            if (i != parameters.Length - 1 && !endingWithComma)
                InterpreterWatcher.PseudoThrow($"Expecting a sequence as parameter at line: {line.Number}");

            NameToken parameter = new NameToken().Parse(new CodeLine(parameters[i].Replace(",", "")));
            p.Add(parameter);
        }

        return p.ToArray();
    }

    public List<string> ToTreeView()
        => new()
        {
            $"Method call: {Name.Value}, parameters: {Parameters.ToInlineString()}"
        };

    public uint Evaluate()
    {
        return 0;
    }
}