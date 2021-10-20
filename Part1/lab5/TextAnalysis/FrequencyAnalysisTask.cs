using System.Collections.Generic;
using System.Linq;
using System;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary <string, Dictionary<string,int>> CreateNgrams (List<List<string>> text, int n)
        {
            var ngramFrequency = new Dictionary<string, Dictionary<string, int>>();
            for (int i = 0; i < text.Count; i++)
            {
                for (int j = 0; j < text[i].Count - n; j++)
                {
                    var keyStart = n==1 ? text[i][j] : (text[i][j] + " " + text[i][j + 1]);
                    var keyEnd = text[i][j + n];
                    if (!ngramFrequency.ContainsKey(keyStart))
                        ngramFrequency[keyStart] = new Dictionary<string, int>();
                    if (!ngramFrequency[keyStart].ContainsKey(keyEnd))
                        ngramFrequency[keyStart][keyEnd] = 1;
                    else
                        ngramFrequency[keyStart][keyEnd]++;
                }
            }
            return ngramFrequency;
        }

        public static Dictionary<string,string> CreateNgramDictionary (Dictionary<string,Dictionary<string,int>> ngramFrequency)
        {
            var result = new Dictionary<string, string>();
            foreach (var startNgram in ngramFrequency)
            {
                var endKey = "";
                var keyValue = 0;
                foreach (var endNgram in ngramFrequency[startNgram.Key])
                {
                    if (endNgram.Value > keyValue)
                    {
                        endKey = endNgram.Key;
                        keyValue = endNgram.Value;
                    }
                    else if (endNgram.Value == keyValue)
                       if( string.CompareOrdinal(endKey, endNgram.Key) > 0)
                            endKey = endNgram.Key;
                }
                result[startNgram.Key] = endKey;
            }
            return result;
        }

        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var bigramFrequency = CreateNgrams(text,1);
            var trigramFrequency = CreateNgrams(text,2);
            var bigramDictionary = CreateNgramDictionary(bigramFrequency);
            var trigramDictionary = CreateNgramDictionary(trigramFrequency);
            bigramDictionary.ToList().ForEach(x => result.Add(x.Key, x.Value));
            trigramDictionary.ToList().ForEach(x => result.Add(x.Key, x.Value));
            return result;
        }
   }
}