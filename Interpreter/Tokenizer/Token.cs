using System;
using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter.Interpreter
{
    public interface IStackable : ITreeViewElement
    {
        
    }

    public interface IToken
    {
        public bool EndsWithSemicolon(ReadOnlySpan<char> phrase) => phrase.EndsWith(";");
        IToken Parse(CodeLine line);
    }
}