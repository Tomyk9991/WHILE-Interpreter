using System.Collections.Generic;
using System.Linq;
using System.Text;
using WHILE_Interpreter.Interpreter.Logging;

namespace WHILE_Interpreter.Interpreter
{
    public static class ExtensionMethods
    {
        public static string ToInlineString<T>(this IEnumerable<T> list) where T : ITreeViewElement
        {
            T lastElement = list.Last();
            
            StringBuilder builder = new StringBuilder("[");

            foreach (T value in list)
            {
                builder.Append(value.ToTreeView()[0]);
                if (!value.Equals(lastElement))
                {
                    builder.Append(", ");
                }
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}