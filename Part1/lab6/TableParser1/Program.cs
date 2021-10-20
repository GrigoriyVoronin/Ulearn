using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TableParser
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            var filename = args.Length > 0 ? args[0] : "data.txt";
            var lines = File.ReadAllLines(filename);
            var rows = lines.Select(FieldsParserTask.ParseLine).ToList();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParserMainForm());
        }
    }
}