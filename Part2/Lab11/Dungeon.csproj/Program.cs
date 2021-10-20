using System;
using System.Windows.Forms;

namespace Dungeon
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new DungeonForm();
            Application.Run(form);
        }
    }
}