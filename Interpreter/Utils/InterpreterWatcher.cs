using System;

namespace WHILE_Interpreter.Interpreter
{
    public static class InterpreterWatcher
    {
        public static bool HasThrown { get; private set; }
        public static void PseudoThrow(string message)
        {
            HasThrown = true;
            Console.WriteLine(message);
        }
    }
}