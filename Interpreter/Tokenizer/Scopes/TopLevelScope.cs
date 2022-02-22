using System;
using System.Collections.Generic;
using System.Text;
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

        public string ToTreeView(int indent = 0)
        {
            StringBuilder builder = new StringBuilder("Scope:");

            builder.Append(' ', indent * 2).AppendLine("{");
            builder.Append(' ', (indent + 1) * 2).AppendLine("Stack: {");
            foreach (IStackable stackable in Stack)
            {
                builder.AppendLine(stackable.ToTreeView(indent + 2));
            }

            builder.Append(' ', (indent + 1) * 2).AppendLine("}");

            builder.Append(' ', (indent + 1) * 2).AppendLine("Methods: {");
            foreach (MethodToken method in Methods)
            {
                builder.AppendLine(method.ToTreeView(indent + 2));
            }
            builder.Append(' ', (indent + 1) * 2).AppendLine("}");
            builder.AppendLine("}");

            return builder.ToString();
        }
    }
}