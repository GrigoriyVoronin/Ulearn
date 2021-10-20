using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minilab5
{
    class Program
    {
        //private static string DecodeMessage(string[] lines)
        //{
        //    var words = new List<string>();
        //    for (int i = 0; i < lines.Length; i++)
        //    {
        //        var wordsInLine = lines[i].Split();
        //        for (int j = 0; j < wordsInLine.Length; j++)
        //            if (wordsInLine[j].Length != 0 && char.IsUpper(wordsInLine[j][0]))
        //                words.Add(wordsInLine[j]);
        //    }
        //    words.Reverse();
        //    return string.Join(" ", words.ToArray());
        //}
        private static string DecodeMessage(string[] lines)
        {
            return string.Join(" ", string.Join(" ", lines).Split().Where(str => str.Length > 0 && char.IsUpper(str[0])).Reverse());
        }
        private static Dictionary<string, List<string>> OptimizeContacts(List<string> contacts)
        {
            var dictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i<contacts.Count; i++)
            {
                var key = contacts[i].Split(':')[0].Substring(0, Math.Min(contacts[i].Split(':')[0].Length, 2));
                
                    if (!dictionary.ContainsKey(key))
                        dictionary[key] = new List<string>();
                    dictionary[key].Add(contacts[i]);
            }
            return dictionary;
        }

        private static string ApplyCommands(string[] commands)
        {
            var text = new StringBuilder();
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].StartsWith("push"))
                    text.Append(commands[i].Substring(5));
                else
                    text.Length -= int.Parse(commands[i].Substring(4));
            }
            return text.ToString();
        }



        static void Main(string[] args)
        {
            Console.WriteLine(DecodeMessage(new[] {"решИла нЕ Упрощать и зашифРОВАтЬ Все послаНИЕ",
"дАже не Старайся нИЧЕГО у тЕбя нЕ получится с расшифРОВкой",
"Сдавайся НЕ твоего ума Ты не споСОбЕн Но может быть",
"если особенно упорно подойдешь к делу",
"",
"будет Трудно конечнО",
"Код ведЬ не из простых",
"очень ХОРОШИЙ код",
"то у тебя все получится",
"и я буДу Писать тЕбЕ еще",
"",
"чао"}));
            foreach(var pair in OptimizeContacts(new List<string> { @"< name >:< email >"}))
            {
                Console.WriteLine(pair.Key);
                foreach (var email in pair.Value)
                    Console.WriteLine(email);
            }
            Console.WriteLine(ApplyCommands(new[] { "push Привет! Это снова я! Пока!",
"pop 5",
"push Как твои успехи? Плохо?",
"push qwertyuiop",
"push 1234567890",
"pop 26"}));
        }
    }
}
