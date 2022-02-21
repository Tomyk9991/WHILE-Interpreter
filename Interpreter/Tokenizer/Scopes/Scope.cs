using System.Collections.Generic;

namespace WHILE_Interpreter.Interpreter
{
    public abstract class Scope
    {
        public Stack<IStackable> Stack {get; set;}

        protected Scope()
        {
            this.Stack = new Stack<IStackable>();
        }
        
        public abstract IToken Parse(CodeLine line);
    }
}