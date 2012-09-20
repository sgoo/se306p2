using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace se306p2
{
    static class Program
    {
        // this is a change
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// Hello its me charu
        [STAThread]
        static void Main()
        {
            // more comments
            Application.EnableVisualStyles();
            // even more comments
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
