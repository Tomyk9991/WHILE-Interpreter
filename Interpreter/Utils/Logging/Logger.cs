namespace WHILE_Interpreter.Interpreter.Logging;

public class Logger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void Log(object message)
    {
        Console.WriteLine(message);
    }
}