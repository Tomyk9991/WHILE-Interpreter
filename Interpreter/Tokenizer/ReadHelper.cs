using System;
using System.IO;
using System.Linq;

namespace WHILE_Interpreter.Interpreter
{
    public class ReadHelper
    {
        public static CodeLine[] Read(string path)
        {
            string[] sourceCode = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "/Program.while");
            CodeLine[] lines = new CodeLine[sourceCode.Length];

            for (int i = 0; i < sourceCode.Length; i++)
            {
                lines[i] = new CodeLine(sourceCode[i], (uint) i + 1);
            }

            return lines;
        }

        public static CodeLine[] Normalize(CodeLine[] sourceCode)
        {
            return sourceCode.Where(line => !string.IsNullOrEmpty(line.Phrase) && !string.IsNullOrWhiteSpace(line.Phrase)).ToArray();
        }
    }

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

        public override string ToString()
        {
            return this.Phrase;
        }
    }
}