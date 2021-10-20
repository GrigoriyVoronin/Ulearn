using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "",2)]
        [TestCase("'a'", 0, "a",3)]
        [TestCase(@"""a 'b' 'c' d"" '""1"" ""2"" ""3""'""", 0, @"a 'b' 'c' d",13)]
        [TestCase (@"'' ""bcd ef"" 'x y'",0, "",2)]
        [TestCase (@"a""b c d e""f",1, "b c d e",9)]
        [TestCase(@"abc ""def g h",4, "def g h",8)]
        [TestCase(@"""a \""c\""""",0, "a \"c\"",9)]
        [TestCase(@"""\\"" b",0,"\\",4)]
        public static void Test(string line, int startIndex, string expectedTokenValue, int expectedLenght)
        {
            var expectedToken = new Token(expectedTokenValue, startIndex, expectedLenght);
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(expectedToken , actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var str = "";
            var index = startIndex + 1;
            var screenCounter = 0;
            while (index != line.Length && line[index] != line[startIndex])
            {
                if (line[index] == '\\')
                {
                    str += line[index + 1];
                    index += 2;
                    screenCounter++;
                }
                else
                {
                    str += line[index];
                    index++;
                }
            }
            var len = str.Length+2+screenCounter;
            if (index >= line.Length && line[line.Length - 1] != line[startIndex])
                len--;
            return new Token(str, startIndex, len);
        }
    }
}
