using System;
using System.IO;
using System.Linq;

namespace WHILE_Interpreter.Interpreter
{
    public class ReadHelper
    {
        public static CodeLine[] Read(string path)
        {
            string[] sourceCode = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + path);
            CodeLine[] lines = new CodeLine[sourceCode.Length];

            for (int i = 0; i < sourceCode.Length; i++)
            {
                lines[i] = new CodeLine(sourceCode[i], (uint) i + 1);
            }
            
            return lines;
        }
        
        public static CodeLine[] Read(string[] sourceCode)
        {
            CodeLine[] lines = new CodeLine[sourceCode.Length];

            for (int i = 0; i < sourceCode.Length; i++)
            {
                lines[i] = new CodeLine(sourceCode[i], (uint) i + 1);
            }

            return lines;
        }

        public static CodeLine[] Normalize(CodeLine[] sourceCode)
        {
            List<CodeLine> source = new List<CodeLine>();
            
            for (int clc = 1, i = 0; i < sourceCode.Length; i++)
            {
                var line = sourceCode[i];
                if (!string.IsNullOrEmpty(line.Phrase) && !string.IsNullOrWhiteSpace(line.Phrase))
                {
                    source.Add(new CodeLine(line.Phrase, (uint)clc));
                    clc++;
                }
            }

            return source.ToArray();
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
        
        public static implicit operator CodeLine(string line) => new(line, 1);

        public override string ToString()
        {
            return $"{Number}: {this.Phrase}";
        }
    }
}