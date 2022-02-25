using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter.Interpreter.Method
{
    public class MethodHeaderToken : IToken, ITreeViewElement
    {
        public NameToken[] Parameters { get; private set; }
        public NameToken Name { get; private set; }
        public TypeToken ReturnType { get; private set; }
        
        public IToken Parse(CodeLine line)
        {
            string[] split = line.Phrase.Split(' ', ':', '(', ')')
                .Where(p => !string.IsNullOrEmpty(p) && !string.IsNullOrWhiteSpace(p)).ToArray();


            if (!line.Phrase.Contains("(") || !line.Phrase.Contains(")"))
                return null;
            
            if (split.Length < 2)
                return null;

            TypeToken typeToken = TypeTokens.Analyse(split[0]);

            if (typeToken == TypeToken.Invalid)
                return null;

            this.ReturnType = typeToken;

            NameToken nameToken = new NameToken().Parse(new CodeLine(split[1]));

            if (nameToken == null)
            {
                InterpreterWatcher.PseudoThrow($"Expected a name for the method at line {line.Number}");
                return null;
            }

            this.Name = nameToken;

            NameToken[] parameters = null;
            if (split.Length > 2)
            {
                parameters = ParseParameters(split[2..], line);
            }

            this.Parameters = parameters;

            return this;
        }

        private NameToken[] ParseParameters(string[] parameters, CodeLine line)
        {
            List<NameToken> p = new List<NameToken>();
            for (int i = 0; i < parameters.Length; i++)
            {
                // all elements except the last element must end with a comma
                bool endingWithComma = parameters[i].EndsWith(",");
                
                if (i != parameters.Length - 1 && !endingWithComma)
                    InterpreterWatcher.PseudoThrow($"Expecting a sequence as parameter at line: {line.Number}");

                NameToken parameter = new NameToken().Parse(new CodeLine(parameters[i].Replace(",", "")));
                p.Add(parameter);
            }

            return p.ToArray();
        }

        public List<string> ToTreeView()
            => new()
            {
                $"name: {Name.Value}, return: {ReturnType}, parameters: {Parameters.ToInlineString()}"
            };
    }
}