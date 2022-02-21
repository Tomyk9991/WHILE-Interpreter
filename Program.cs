using System;
using WHILE_Interpreter.Interpreter;

namespace WHILE_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeLine[] sourceCode = ReadHelper.Read(AppDomain.CurrentDomain.BaseDirectory + "/Program.while");
            sourceCode = ReadHelper.Normalize(sourceCode);
            
            Tokenizer tokenizer = new Tokenizer();
            tokenizer.Tokenize(sourceCode);
            
            // Console.ReadLine();
        }
    }
}