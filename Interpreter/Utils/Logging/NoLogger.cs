namespace WHILE_Interpreter.Interpreter.Logging;

public class NoLogger : ILogger
{
    public void Log(string message) { }
    public void Log(object message) { }
}

public interface ILogger
{
    void Log(string message);
    void Log(object message);
}