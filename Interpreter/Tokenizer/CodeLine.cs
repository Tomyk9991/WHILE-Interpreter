namespace WHILE_Interpreter.Interpreter;

public class CodeLine
{
    public string Phrase { get; private set; }
    public uint Number { get; private set; }

    public CodeLine(string phrase, uint number)
    {
        Phrase = phrase;
        Number = number;
    }

    public CodeLine(string value)
    {
        Phrase = value;
        Number = 0;
    }
        
    public static implicit operator CodeLine(string line) => new(line, 1);

    public override string ToString()
    {
        return $"{Number}: {this.Phrase}";
    }
}