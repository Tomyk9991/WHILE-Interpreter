using WHILE_Interpreter.Interpreter;
namespace WHILE_Interpreter;

class Program
{
    /*
     */
    public static void Main(string[] args)
    {
        CodeLine[] sourceCode = ReadHelper.Read(AppDomain.CurrentDomain.BaseDirectory + "/Program.while");
        sourceCode = ReadHelper.Normalize(sourceCode);
            
        Tokenizer tokenizer = new Tokenizer();
        TopLevelScope scope = tokenizer.Tokenize(sourceCode);

        Console.ReadLine();
    }
}

