using WHILE_Interpreter.Interpreter;
using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Method;

namespace Tests.TokenizerTests;

[TestClass]
public class MethodTests
{
    [TestMethod]
    public void MethodHeaderToken()
    {
        StringResultPair[] tests =
        {
            new("void hallo():", true),
            new("void this_is_a_long_method():", true),
            new("void this_is_a_long_method()", false),
            new("num hallo():", true),
            new("void hallo():", true),
            
            new("num againLongMethodName(a, b):", true),
            new("num againLongMethodName(a, 5):", false),
            new("void againLongMethodName(a, b324s):", true),
            
            new("void againLongMethodName(a, 1231a):", false),
        };

        
        foreach (var pair in tests)
        {
            var line = new CodeLine(pair.Token);
            var token = new MethodHeaderToken().Parse(line);

            if (pair.Result)
                Assert.IsNotNull(token, pair.Token);
            else
                Assert.IsNull(token, pair.Token);
        }
    }
    
    [TestMethod]
    public void CompleteMethodTests()
    {
        CodeLineArrayMethodResultPair[] tests =
        {
            new(new CodeLine[] 
            {
                new("void hallo1():", 1),
                new("    a = 5;", 2),
                new("    return;", 3)
            }, 1),
            new(new CodeLine[] 
            {
                new("void hallo2():", 1),
                new("    a = 5;", 2)
            }, 0),
            new(new CodeLine[] 
            {
                new("void hallo3():", 1),
                new("    a = 5;", 2),
                new("    return;", 3),
                new("void welt():", 4),
                new("    b = 6;", 5),
                new("    return;", 6),
            }, 2),
            new(new CodeLine[] 
            {
                new("void hallo4():", 1),
                new("void welt():", 2),
                new("    b = 6;", 3),
                new("    return;", 4),
            }, 0),
            
            new(new CodeLine[] 
            {
                new("num welt5():", 1),
                new("    b = 6;", 2),
                new("    return 5;", 3),
            }, 1),
            
            new(new CodeLine[] 
            {
                new("num welt6():", 1),
                new("    b = 6;", 2),
                new("    return welt();", 3),
            }, 1)
        };

        foreach (var pair in tests)
        {
            Tokenizer tokenizer = new Tokenizer(new NoLogger());
            TopLevelScope scope = tokenizer.Tokenize(pair.CodeLines);

            Assert.AreEqual(pair.Result, scope.Methods.Count, pair.CodeLines[0].Phrase);
        }
    }
}