﻿using System;
using System.Windows.Forms;

namespace Void.UI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var view = new MainForm();
            Application.Run(view);
        }
    }
}
