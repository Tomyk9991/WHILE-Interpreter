using WHILE_Interpreter.Interpreter;
using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Operators;
using WHILE_Interpreter.Interpreter.While;

namespace Tests.TokenizerTests;

[TestClass]
public class WhileTests
{
    [TestMethod]
    public void SimpleWhile()
    {
        var tests = new[]
        {
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("    a += 1;", 2),
                new("#", 3),
            }, 1)
        };
        
        Tokenizer tokenizer = new Tokenizer(new NoLogger());
        TopLevelScope scope = tokenizer.Tokenize(tests[0].Lines);

        WhileHeaderToken whileHeaderToken = new WhileHeaderToken(new NameToken()
        {
            Value = "a"
        });

        WhileToken token = new WhileToken(whileHeaderToken, null)
        {
            Scope = new InnerBodyScope(null, null)
        };
        
        token.Scope.Stack.Push((AdditiveOperatorToken) new AdditiveOperatorToken().Parse("a += 1;"));
        
        Stack<IStackable> expectedScopeTopLevel = new Stack<IStackable>();
        expectedScopeTopLevel.Push(token);

        Assert.AreEqual(expectedScopeTopLevel.First().ToTreeView().TS(), scope.Stack.First().ToTreeView().TS());
    }
    
    [TestMethod]
    public void SimpleWhile2()
    {
        var tests = new[]
        {
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("    a += 1;", 2),
            }, 0),
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("    a += 1;", 2),
                new("    b += 1;", 3),
                new("    c += 1;", 4),
                new("    super_long_method_call();", 5),
                new("#", 5),
            }, 1),
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("#", 2),
            }, 1),
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("  #", 2),
            }, 1)
        };

        foreach (var test in tests)
        {
            Tokenizer tokenizer = new Tokenizer(new NoLogger());
            TopLevelScope scope = tokenizer.Tokenize(test.Lines);
        
            Assert.AreEqual(test.Nesting, scope.Stack.Count);
        }
    }
    
    [TestMethod]
    public void NestedWhileLoops()
    {
        var tests = new[]
        {
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("    while a != 0:", 2),
                new("    #", 3),
                new("#", 4),
            }, 2),
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("    while b != 0:", 2),
                new("        while c != 0:", 3),
                new("            while d != 0:", 4),
                new("    #", 5),
                new("#", 6),
            }, 0),
            new CodeLineArrayNestedWhileLoopPair(new CodeLine[]
            {
                new("while a != 0:", 1),
                new("    while a != 0:", 2),
                new("        while c != 0:", 3),
                new("            while d != 0:", 4),
                new("            #", 5),
                new("        #", 6),
                new("    #", 7),
                new("#", 8),
            }, 4)
            
        };
        
        foreach (var test in tests)
        {
            Tokenizer tokenizer = new Tokenizer(new NoLogger());
            TopLevelScope scope = tokenizer.Tokenize(test.Lines);

            int nestingCount = 0;

            
            WhileToken currentWhileToken = test.Nesting > 0 ? (WhileToken)scope.Stack.First() : null;
            
            while (currentWhileToken != null)
            {
                nestingCount++;
                currentWhileToken = currentWhileToken.Scope.Stack.SearchType<WhileToken>();
            }
            
            Assert.AreEqual(test.Nesting, nestingCount);
        }
    }
}