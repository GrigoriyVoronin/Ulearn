using System.Collections.Generic;
using NUnit.Framework;
using System;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (var i = 0; i < expectedResult.Length; ++i) Assert.AreEqual(expectedResult[i], actualResult[i].Value);
        }

        [TestCase("a", new[] { "a" })] // ���� ����
        [TestCase("a b", new[] { "a", "b" })] //������ ������ ���� & ����������� ������ � ���� ������
        [TestCase("", new string[] { })] //��� �����
        [TestCase("a  b", new[] { "a", "b" })] //����������� ������ >1 �������
        [TestCase("''", new[] { "" })] //������ ����
        [TestCase("a ''", new[] { "a", "" })] //���� � �������� ����� �������� ����
        [TestCase("''a", new[] { "", "a" })] //����������� ��� �������� & ������� ���� ����� ���� � ��������
        [TestCase("\"", new[] { "" })] //��� ����������� �������
        [TestCase("\"\\\"\"", new[] { "\"" })] //�������������� ������� ������� ������ �������
        [TestCase("\"\\\'\"", new[] { "\'" })] // ��������� ������� ������ �������
        [TestCase("\'\\\"\'", new[] { "\"" })] //������� ������� ������ ���������
        [TestCase("\'\\\'\'", new[] { "\'" })] //�������������� ��������� ������� ������ ���������
        [TestCase("' '", new[] { " " })] //������ ������ �������
        [TestCase("\"\\\\\"", new[] { "\\" })] // �������������� �������� ���� ������ ������� & �������������� �������� ���� ����� ����������� ��������
        [TestCase(" a", new[] { "a" })] //������� � ������ ��� � ����� ������
        [TestCase("\" ", new[] { " " })] // ������ � ����� ���� � ���������� ��������
        [TestCase("a \"bcd ef\" 'x y'",new [] { "a", "bcd ef", "x y" })]
        [TestCase ("a\"b c d e\"f",new[] { "a", "b c d e", "f" })]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static Token ReadUsualField(string line, int startIndex)
        {
            var index = startIndex;
            var symbols = new[] { ' ', '"', '\'' };
            while (index < line.Length && Array.IndexOf(symbols,line[index]) == -1) index++;
            index -= startIndex;
            return new Token(line.Substring(startIndex, index), startIndex, index);
        }

        public static List<Token> ParseLine(string line)
        {
            var tokens = new List<Token>();
            var index = 0;
            while (index < line.Length)
                if (line[index] == ' ')
                {
                    index++;
                }
                else if (line[index]!='\'' && line[index]!='"')
                {
                    tokens.Add(ReadUsualField(line, index));
                    index = tokens[tokens.Count - 1].GetIndexNextToToken();
                }
                else
                {
                    tokens.Add(QuotedFieldTask.ReadQuotedField(line, index));
                    index = tokens[tokens.Count - 1].GetIndexNextToToken();
                }

            return  tokens; 
        }
    }
}