using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private readonly Dictionary<int, Dictionary<string, List<int>>> positions =
            new Dictionary<int, Dictionary<string, List<int>>>(8);
        private readonly Dictionary<string, HashSet<int>> wordInDocuments =
            new Dictionary<string, HashSet<int>>(8);
        private readonly char[] delimiters = new[] { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };


        public void Add(int id, string documentText)
        {
            positions[id] = new Dictionary<string, List<int>>(8);
            var wordsArr = documentText.Split(delimiters);
            var arrLen = wordsArr.Length;
            var len = 0;
            for (int i = 0; i < arrLen; i++)
            {
                var word = wordsArr[i];

                if (!wordInDocuments.ContainsKey(word))
                    wordInDocuments[word] = new HashSet<int>(8);

                if (!wordInDocuments[word].Contains(id))
                    wordInDocuments[word].Add(id);

                if (!positions[id].ContainsKey(word))
                    positions[id][word] = new List<int>(8);

                positions[id][word].Add(len);
                len += word.Length + 1;
            }
        }

        public List<int> GetIds(string word)
        {
            HashSet<int> idDic;
            wordInDocuments.TryGetValue(word, out idDic);
            return idDic==null ? new List<int>() : idDic.ToList();
        }

        public List<int> GetPositions(int id, string word)
        {
            List<int> idList;
            positions[id].TryGetValue(word, out idList);
            return idList ?? new List<int>();
        }

        public void Remove(int id)
        {
            var words = positions[id].Keys.ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                if (wordInDocuments.ContainsKey(words[i]))
                    wordInDocuments[words[i]].Remove(id);
            }
            positions.Remove(id);
        }
    }
}

