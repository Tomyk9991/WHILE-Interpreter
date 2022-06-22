using WHILE_Interpreter.Interpreter;
using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter;

class Program
{
    public static void Main(string[] args)
    {
        CodeLine[] sourceCode = ReadHelper.Read("/Program.while");
        sourceCode = ReadHelper.Normalize(sourceCode);
        
        // ILogger logger = new NoLogger();
        ILogger logger = new Logger();
        
        
        Tokenizer tokenizer = new Tokenizer(logger);
        TopLevelScope scope = tokenizer.Tokenize(sourceCode);
        
        RunTime runTime = new RunTime(scope, new Logger());
        runTime.Run();
        
        Console.ReadLine();
    }
}