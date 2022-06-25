using System.Text;
using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Operators;

namespace WHILE_Interpreter.Interpreter
{
    public static class ExtensionMethods
    {
        public static T SearchType<T>(this Stack<IStackable> stack)
        {
            foreach (IStackable stackable in stack)
            {
                if (stackable is T target)
                {
                    return target;
                }
            }

            return default;
        }
        
        public static string TS<T>(this IEnumerable<T> list)
        {
            if (list == null) return "[NULL]";
            return string.Join(", ", list);
        }
        
        public static void Print<T>(this IEnumerable<T> list)
        {
            if (list == null)
                Console.WriteLine("[NULL]");
            
            foreach (T value in list)
            {
                Console.WriteLine(value);
            }
        }
        public static string ToInlineString<T>(this IEnumerable<T> list) where T : ITreeViewElement
        {
            if (list == null)
                return "[]";
            
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

        public static string ToStringRepresentation(this Operator op)
        {
            return op switch
            {
                Operator.Add => "+=",
                Operator.Sub => "-=",
                Operator.Noop => "NOOP"
            };
        }
        
        public static string ToMultiLineString<T>(this IEnumerable<T> list) where T : ITreeViewElement
        {
            T lastElement = list.Last();
            
            StringBuilder builder = new StringBuilder("[");
            builder.AppendLine();

            foreach (T value in list)
            {
                builder.Append("\t").Append(value.ToTreeView()[0]);
                if (!value.Equals(lastElement))
                {
                    builder.AppendLine(", ");
                }
            }

            builder.AppendLine();
            builder.Append("]");

            return builder.ToString();
        }
    }
}