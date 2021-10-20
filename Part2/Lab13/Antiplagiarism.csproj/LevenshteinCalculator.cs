using System;
using System.Collections.Generic;
// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var comparisonResult = new List<ComparisonResult>();
            for (var i = 0; i < documents.Count; i++)
            for (var j = i + 1; j < documents.Count; j++)
                comparisonResult.Add(CompareTwoDocuments(documents[i], documents[j]));

            return comparisonResult;
        }

        private static ComparisonResult CompareTwoDocuments(DocumentTokens firstDoc, DocumentTokens secondDoc)
        {
            var optOld = new double[secondDoc.Count + 1];
            var optNew = new double[secondDoc.Count + 1];
            optNew[0] = 1;
            for (var i = 0; i <= secondDoc.Count; ++i)
                optOld[i] = i;
            FindLevenshteinDistanse(firstDoc,secondDoc,optOld,optNew);
            return new ComparisonResult(firstDoc, secondDoc, optNew[secondDoc.Count]);
        }

        private static void FindLevenshteinDistanse(DocumentTokens firstDoc, DocumentTokens secondDoc, double[] optOld, double[]optNew)
        {
            for (var i = 1; i <= firstDoc.Count; ++i)
            {
                for (var j = 1; j <= secondDoc.Count; ++j)
                    if (firstDoc[i - 1] == secondDoc[j - 1])
                        optNew[j] = optOld[j - 1];
                    else
                        optNew[j] = GetMinOfThree(optOld[j] + 1,
                            optOld[j - 1] +
                            TokenDistanceCalculator.GetTokenDistance(firstDoc[i - 1], secondDoc[j - 1]),
                            optNew[j - 1] +
                            TokenDistanceCalculator.GetTokenDistance(firstDoc[i - 1], secondDoc[j - 1]));
                optNew.CopyTo(optOld, 0);
                optNew[0] = i + 1;
            }
        }

        private static double GetMinOfThree(double a, double b, double c) => Math.Min(a, Math.Min(b, c));
    }
}