using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;

namespace WHILE_Interpreter.Interpreter
{
    public class TopLevelScope : Scope, ITreeViewElement
    {
        public List<MethodToken> Methods { get; set; }
        
        public TopLevelScope()
        {
            this.Methods = new List<MethodToken>();
        }

        public override IToken Parse(CodeLine line)
        {
            IToken variableToken = new VariableToken().Parse(line);

            if (variableToken != null)
                return variableToken;

            IToken methodHeader = new MethodHeaderToken().Parse(line);
            if (methodHeader != null)
                return methodHeader;
            
            return null;
        }

        public void Print()
        {
            var lines = ToTreeView();
            foreach (string line in lines)
                Console.WriteLine(line);
        }

        public List<string> ToTreeView()
        {
            List<string> lines = new List<string>
            {
                "Program:", 
                "├── Methods:"
            };

            foreach (MethodToken method in Methods)
            {
                List<string> methodLines = method.ToTreeView();
                lines.Add($"│  ├── Method token: {method.HeaderToken.Name.Value.ToUpper()}");
                
                foreach (string methodLine in methodLines)
                {
                    lines.Add("│  │  " + methodLine);
                }
            }
            
            lines.Add("├── Scope:");
            
            foreach (IStackable stackable in Stack)
            {
                List<string> stackableLines = stackable.ToTreeView();

                foreach (string stackableLine in stackableLines)
                {
                    if (stackableLines.Count == 1)
                        lines.Add("│  ├── " + stackableLine);
                    else
                        lines.Add("│  │  " + stackableLine);
                }
                
            }

            return lines;
        }
    }
}