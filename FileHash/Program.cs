using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sha512
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MessageBox.Show("Valid filename must be passed", "File SHA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(args[0]))
            {
                MessageBox.Show($"'{args[0]}' is not a valid filename", "File SHA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(args[0]));
        }
    }
}
