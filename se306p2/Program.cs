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
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
