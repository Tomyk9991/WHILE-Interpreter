using System;

namespace WHILE_Interpreter.Interpreter.Method
{
    public enum TypeToken
    {
        Void = 0,
        Num = 1,
        Invalid = 2
    }

    public static class TypeTokens
    {
        public static TypeToken Analyse(string word)
        {
            string[] e = Enum.GetNames(typeof(TypeToken));
            
            for (int i = 0; i < e.Length - 1; i++)
            {
                var enumString = e[i];
                
                if (string.Equals(word, enumString, StringComparison.CurrentCultureIgnoreCase))
                    return (TypeToken) i;
            }

            return TypeToken.Invalid;
        }
    }
}