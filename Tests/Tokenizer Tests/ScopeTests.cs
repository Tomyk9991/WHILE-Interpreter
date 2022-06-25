using System.Collections;
using System.Text.Json;
using WHILE_Interpreter.Interpreter;
using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;

namespace Tests.TokenizerTests;

[TestClass]
public class ScopeTests
{
    [TestMethod]
    public void TestInnerBodyScope()
    {
        CodeLineArrayMethodResultPair[] tests = new[]
        {
            new CodeLineArrayMethodResultPair(new CodeLine[]
            {
                new("void main():", 1),
                new("    a = 5;", 2),
                new("    b = 5;", 3),
                new("    return;", 4),
            }, 1)
        };

        foreach (var test in tests)
        {
            Tokenizer tokenizer = new Tokenizer(new NoLogger());
            TopLevelScope scope = tokenizer.Tokenize(test.CodeLines);

            Stack<IStackable> expectedScope = new Stack<IStackable>();


            MethodHeaderToken expectedMethodHeaderToken =
                (MethodHeaderToken)new MethodHeaderToken().Parse("void main():");
            
            expectedScope.Push((IStackable) new VariableToken().Parse("a = 5;"));
            expectedScope.Push((IStackable) new VariableToken().Parse("b = 5;"));
            expectedScope.Push(new ReturnToken(expectedMethodHeaderToken));
            
            
            CollectionAssert.AreEqual(scope.Methods[0].Scope.Stack, expectedScope, new IStackableComparer());
            
            Console.WriteLine(scope);
        }
    }
}

public class IStackableComparer : IComparer
{
    public int Compare(object? x, object? y)
    {
        if (x is null && y is null)
            return 0;
        if (x is null)
            return -1;
        if (y is null)
            return 1;

        string serializedX = JsonSerializer.Serialize(x);
        string serializedY = JsonSerializer.Serialize(y);
        
        return string.Compare(serializedX, serializedY, StringComparison.Ordinal);
    }
}