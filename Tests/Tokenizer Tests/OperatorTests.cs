using WHILE_Interpreter.Interpreter;
using WHILE_Interpreter.Interpreter.Operators;

namespace Tests.TokenizerTests;

[TestClass]
public class OperatorTests
{
    private StringResultPair[] testCases = new StringResultPair[6]
    {
        new("a #= b;", true),
        new("a #= 5", false),
        new("a #= 5;", true),
        new("a #= add(123, 4102);", true),
        new("a #= super_long_test();", true),
        new("a #= ;", false),
    };

    private void ReplaceCharInStringResultPairArrayWith(char old, char character)
    {
        for (int i = 0; i < testCases.Length; i++)
        {
            testCases[i] = new StringResultPair(testCases[i].Token.Replace(old, character), testCases[i].Result);
        }
    }
    
    [TestMethod]
    public void AdditiveOperator()
    {
        ReplaceCharInStringResultPairArrayWith('#', '+');
        

        foreach (StringResultPair test in testCases)
        {
            CodeLine line = new CodeLine(test.Token);
            var token = (AdditiveOperatorToken)new AdditiveOperatorToken().Parse(line);

            if (test.Result)
                Assert.IsNotNull(token, test.Token);
            else
                Assert.IsNull(token, test.Token);
        }
        
        ReplaceCharInStringResultPairArrayWith('+', '#');
    }

    [TestMethod]
    public void SubtractiveOperator()
    {
        ReplaceCharInStringResultPairArrayWith('#', '-');
        foreach (StringResultPair test in testCases)
        {
            CodeLine line = new CodeLine(test.Token);
            var token = (AdditiveOperatorToken)new AdditiveOperatorToken().Parse(line);

            if (test.Result)
                Assert.IsNotNull(token, test.Token);
            else
                Assert.IsNull(token, test.Token);
        }
        
        ReplaceCharInStringResultPairArrayWith('-', '#');
    }
}

