using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Antiplagiarism
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Идёт анализ решений...");
            var folder = Folders.SuspiciousSources;
            if (args.Length != 0)
            {
                folder = new DirectoryInfo(args[0]);
            }
            var documents = DocumentLoader.LoadAllStateNames(folder)
                .Select(documentName => new DocumentContent(documentName))
                .ToList();
            var levenshteinCalculator = new LevenshteinCalculator();
            var comparisonResults = levenshteinCalculator
                .CompareDocumentsPairwise(documents
                    .Select(d => d.Tokens)
                    .ToList()
                )
                .OrderBy(GetNormalizedDistance)
                .Take(5);
            Console.WriteLine("Анализ окончен\n Топ-5 самых похожих пар:\n");

            GenerateReport(documents, comparisonResults);
        }

        private static void GenerateReport(List<DocumentContent> documents, IEnumerable<ComparisonResult> comparisonResults)
        {
            bool isNotImplemented = false;
            var stringWriter = new StringWriter();
            stringWriter.WriteLine(HtmlBegin);
            using (var writer = new HtmlTextWriter(stringWriter))
            {
                foreach (var comparisonResult in comparisonResults)
                {
                    var first = documents.Find(d => d.Tokens == comparisonResult.Document1);
                    var second = documents.Find(d => d.Tokens == comparisonResult.Document2);
                    var normalizedDistanze = GetNormalizedDistance(comparisonResult);
                    Console.WriteLine(
                        $"Расширенное расстояние Левенштейна между \"{first.DocumentName}\" и \"{second.DocumentName}\" равно {normalizedDistanze}");
                    List<string> commonSequence;
                    try
                    {
                        commonSequence = LongestCommonSubsequenceCalculator.Calculate(first.Tokens, second.Tokens);
                    }
                    catch (NotImplementedException)
                    {
                        isNotImplemented = true;
                        continue;
                    }

                    SaveResult(first, second, commonSequence, writer);
                }
            }
            stringWriter.WriteLine(HtmlEnd);

            if (isNotImplemented)
            {
                Console.WriteLine("Для генерации отчёта реализуйте класс \"LondestCommonSubsequenceCalculator\"");
            }
            else
            {
                var fileName = Path.Combine(Folders.ComparisonResults.FullName);
                File.WriteAllText(fileName, stringWriter.ToString());
                Console.WriteLine($"Отчёт находится в файле:\n{Folders.ComparisonResults}");
            }
        }

        private static double GetNormalizedDistance(ComparisonResult comparisonResult)
        {
            return 2 * comparisonResult.Distance / (comparisonResult.Document1.Count + comparisonResult.Document2.Count);
        }

        private static void SaveResult(DocumentContent first, DocumentContent second, List<string> commonSequence, HtmlTextWriter writer)
        {
            WriteDocumentsNames(first.DocumentName, second.DocumentName, writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            WriteDocumentInHtml(first, commonSequence, writer);
            WriteDocumentInHtml(second, commonSequence, writer);
            writer.RenderEndTag();
        }

        private static void WriteDocumentsNames(string firstDocumentName, string secondDocumentName, HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.WriteEncodedText(firstDocumentName);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.WriteEncodedText(secondDocumentName);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        private static void WriteDocumentInHtml(DocumentContent document, List<string> commonSequence, HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            bool wasCommon = false;
            var whiteSpacesTokens = new StringBuilder();
            foreach (var tokenAndType in document.DevideToCommonAndSpecificTokens(commonSequence))
            {
                var tag = tokenAndType.Item2;
                var token = tokenAndType.Item1;
                if (token.Any(char.IsWhiteSpace))
                {
                    whiteSpacesTokens.Append(token);
                    continue;
                }
                if (wasCommon && tag == TokenType.Common)
                {
                    WriteWithTag(writer, whiteSpacesTokens.ToString(), TokenType.Common);
                }
                else
                {
                    WriteWithTag(writer, whiteSpacesTokens.ToString(), TokenType.Specific);
                }
                WriteWithTag(writer, token, tag);
                whiteSpacesTokens.Clear();
                wasCommon = tag == TokenType.Common;
            }
            WriteWithTag(writer, whiteSpacesTokens.ToString(), TokenType.Specific);
            writer.RenderEndTag();
        }

        private static void WriteWithTag(HtmlTextWriter writer, string text, TokenType tag)
        {
            if (text.Length == 0) 
                return;
            writer.AddAttribute(HtmlTextWriterAttribute.Class, tag.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.WriteEncodedText(text);
            writer.RenderEndTag();
        }

        private const string HtmlBegin =
            @"<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8""/>
<title>Похожие решения</title>
<style type=""text/css"">
* {padding-top:0;margin-top:0;border-top:0;}
.Common {background-color: #ff9999;}
tr, td {vertical-align: top; border-left: 1px solid #ddd;border-bottom: 1px solid #ddd;}
th {background-color: #ddd} 
</style>
</head>
<body>
<pre>
<table valign=""top"">";

        private const string HtmlEnd = @"</table></pre></body></html>";
    }
}
