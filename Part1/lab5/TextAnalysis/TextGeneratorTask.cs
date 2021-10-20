using System.Collections.Generic;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string CreateNextWord (List <string> words, Dictionary<string, string> nextWords)
        {
            if (words.Count > 1)
            {
                var endPhrase = words[words.Count - 2] + " " + words[words.Count - 1];
                if (nextWords.ContainsKey(endPhrase))
                {
                    return nextWords[endPhrase];
                }
                else if (nextWords.ContainsKey(words[words.Count - 1]))
                {
                    return nextWords[words[words.Count - 1]];
                }
                else
                    return "";
            }
            else
            {
                if (nextWords.ContainsKey(words[words.Count - 1]))
                {
                   return nextWords[words[words.Count - 1]];
                }
                else
                    return "";
            }
        }
        public static string ContinuePhrase(Dictionary<string, string> nextWords, string phraseBeginning, int wordsCount)
        {
            var words = SentencesParserTask.CreateSentense(phraseBeginning);
            var startLenght = words.Count;
            while (words.Count < startLenght + wordsCount)
            {
                var word = CreateNextWord(words, nextWords);
                if (word != "")
                    words.Add(word);
                else
                    break;
            }
            return string.Join(" ",words);
        }
    }
}