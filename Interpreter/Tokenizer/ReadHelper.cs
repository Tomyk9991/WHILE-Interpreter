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
                
                if (line.Phrase.Trim().StartsWith("//"))
                    continue;
                
                if (!string.IsNullOrEmpty(line.Phrase) && !string.IsNullOrWhiteSpace(line.Phrase))
                {
                    source.Add(new CodeLine(line.Phrase, (uint)clc));
                    clc++;
                }
            }

            return source.ToArray();
        }
    }
}