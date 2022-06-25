using System.Collections;
using WHILE_Interpreter.Interpreter;

namespace Tests;

[TestClass]
public class ReadHelperTests
{
    private readonly string[] testCode;

    public ReadHelperTests()
    {
        testCode = new[]
        {
            "x = 5;", "", "num Add(x, y):",
            "    z = x;", "    z += y;", "    return z;",
            "", "num div(x, y, z):", "    while a != 0:", "        while a != 0:",
            "            b += 5;", "        #", "        quertz = 5;", "    #",
            "    return b;", "", "x += 2;",
            "y = 3;", "", "while a != 0:",
            "    while a != 0:", "        b += 5;", "    #",
            "    quertz = 5;", "#", "",
            "y += x;", "z = add(x, y);", "",
            "WriteLine(z);",
        };
    }

    [TestMethod]
    public void AllLinesArentEmpty()
    {
        CodeLine[] sourceCode = ReadHelper.Read("/Program.while");
        sourceCode = ReadHelper.Normalize(sourceCode);

        foreach (var codeLine in sourceCode)
        {
            Assert.AreNotEqual(codeLine.Phrase, "");
        }
    }

    [TestMethod]
    public void LinesNormalized()
    {
        CodeLine[] sourceCode = ReadHelper.Read(this.testCode);
        sourceCode = ReadHelper.Normalize(sourceCode);

        CodeLine[] expectedCodeLines = new CodeLine[24]
        {
            new("x = 5;", 1),
            new("num Add(x, y):", 2),
            new("    z = x;", 3),
            new("    z += y;", 4),
            new("    return z;", 5),
            new("num div(x, y, z):", 6),
            new("    while a != 0:", 7),
            new("        while a != 0:", 8),
            new("            b += 5;", 9),
            new("        #", 10),
            new("        quertz = 5;", 11),
            new("    #", 12),
            new("    return b;", 13),
            new("x += 2;", 14),
            new("y = 3;", 15),
            new("while a != 0:", 16),
            new("    while a != 0:", 17),
            new("        b += 5;", 18),
            new("    #", 19),
            new("    quertz = 5;", 20),
            new("#", 21),
            new("y += x;", 22),
            new("z = add(x, y);", 23),
            new("WriteLine(z);", 24),
        };

        CollectionAssert.AreEqual(expectedCodeLines, sourceCode, new CodeLineComparer());
    }
    
    [TestMethod]
    public void LinesCommented()
    {
        string[] commentCode = new[]
        {
            "// x = 5;", 
            "", 
            "//num Add(x, y):",
            "    //////z = x;", 
            "//    //z += y;", 
            "//return z;",
        };
        
        CodeLine[] sourceCode = ReadHelper.Read(commentCode);
        sourceCode = ReadHelper.Normalize(sourceCode);

        CodeLine[] expectedCodeLines = Array.Empty<CodeLine>();

        CollectionAssert.AreEqual(expectedCodeLines, sourceCode, new CodeLineComparer());
    }

    class CodeLineComparer : IComparer
    {
        private static int Compare(CodeLine x, CodeLine y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            var phraseComparison = string.Compare(x.Phrase, y.Phrase, StringComparison.Ordinal);
            return phraseComparison != 0 ? phraseComparison : x.Number.CompareTo(y.Number);
        }

        public int Compare(object? x, object? y)
        {
            return Compare((CodeLine)x, (CodeLine)y);
        }
    }
}