using System;
using System.Windows.Forms;

namespace Rivals
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new RivalsForm();
            Application.Run(form);
        }
    }
}
