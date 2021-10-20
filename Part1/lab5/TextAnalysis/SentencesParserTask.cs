using System.Collections.Generic;
using System;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<string> CreateSentense (string sentenseFromText)
        {
            var sentense = new List<string>();
            var word = "";
            for (int j = 0; j < sentenseFromText.Length; j++)
            {

                if (char.IsLetter(sentenseFromText[j]) || sentenseFromText[j] == '\'')
                {
                    word += sentenseFromText[j];
                    if (j == sentenseFromText.Length - 1)
                    {
                        sentense.Add(word.ToLower());
                        word = "";
                    }
                }
                else
                {
                    if (word != "")
                    {
                        sentense.Add(word.ToLower());
                        word = "";
                    }
                }
            }
            return sentense;
        }
        public static List<List<string>> ParseSentences(string text)
        {
            var sentencesList = new List<List<string>>();
            var delimiters = new char[] { '.', '!', '?', ';', ':', '(', ')' };
            var arrayOfSentenses = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int i =0; i < arrayOfSentenses.Length; i++)
            {
                var sentense = CreateSentense(arrayOfSentenses[i]);
                if (sentense.Count != 0)
                    sentencesList.Add(sentense);
            }
            return sentencesList;
        }
    }
}