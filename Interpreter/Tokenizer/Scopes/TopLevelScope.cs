using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;
using WHILE_Interpreter.Interpreter.Operators;
using WHILE_Interpreter.Interpreter.While;

namespace WHILE_Interpreter.Interpreter
{
    public class TopLevelScope : Scope, ITreeViewElement
    {
        public List<MethodToken> Methods { get; set; }
        public ILogger Logger { get; set; }
        
        public TopLevelScope(ILogger logger)
        {
            this.Methods = new List<MethodToken>();
            this.Logger = logger;
        }

        public override IToken Parse(CodeLine line)
        {
            // Allowed tokens for top level statements
            return new IToken[] {
                new VariableToken(),
                new MethodHeaderToken(),
                new AdditiveOperatorToken(),
                new WhileHeaderToken(),
                new MethodCallToken()
            }.Select(t => t.Parse(line)).FirstOrDefault(token => token != null);
        }

        public void Print()
        {
            var lines = ToTreeView();
            foreach (string line in lines)
                Logger.Log(line);
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

                int counter = 0;
                foreach (string stackableLine in stackableLines)
                {
                    if (stackableLines.Count == 1)
                        lines.Add("│  ├── " + stackableLine);
                    else
                        lines.Add(counter == 0 ? ("│  " + stackableLine) : "│  │" + stackableLine);

                    counter++;
                }
                
            }

            return lines;
        }
    }
}