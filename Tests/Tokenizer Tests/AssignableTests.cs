using WHILE_Interpreter.Interpreter;
using WHILE_Interpreter.Interpreter.Method;

namespace Tests.TokenizerTests;

[TestClass]
public class AssignableTests
{
    [TestMethod]
    public void NameToken()
    {
        StringResultPair[] tests =
        {
            new("hallo", true),
            new("5", false),
            new("a5232a", true),
            new("5a", false),
            new("^^a", false),
            new("while", false)
        };

        
        foreach (var pair in tests)
        {
            var line = new CodeLine(pair.Token);
            var token = new NameToken().Parse(line);

            if (pair.Result)
                Assert.AreEqual(pair.Token, token.Value, pair.Token);
            else
                Assert.AreEqual(null, token, pair.Token);
        }
    }

    [TestMethod]
    public void MethodCallToken()
    {
        StringResultPair[] tests =
        {
            new("add(1, 3)", true),
            new("add()", true),
            new("subtract(5, 5, 2, 5, 3,23)", true),
            new("super_long_method()", true),
            new("while(", false),
            new("while)", false),
            new("5.5.5", false),
            new("-1", false),
            new("14", false),
            new("1231251", false),
        };
        
        foreach (var pair in tests)
        {
            var line = new CodeLine(pair.Token);
            var token = new MethodCallToken().Parse(line);
            
            
            if (pair.Result)
                Assert.IsNotNull(token, pair.Token);
            else
            {
                Assert.IsNull(token, pair.Token);
            }
        }
    }
    
    [TestMethod]
    public void DigitToken()
    {
        StringResultPair[] tests =
        {
            new("5", true),
            new("a5232a", false),
            new("5a", false),
            new("^^a", false),
            new("while", false),
            new("5.5", false),
            new("5.5.5", false),
            new("-1", false),
            new("14", true),
            new("1231251", true),

        };
        
        foreach (var pair in tests)
        {
            var line = new CodeLine(pair.Token);
            var token = new DigitToken().Parse(line);
            
            
            if (pair.Result)
                Assert.AreEqual(uint.Parse(pair.Token), token.Value, message: pair.Token);
            else
                Assert.AreEqual(null, token, message: pair.Token);
        }
    }
}