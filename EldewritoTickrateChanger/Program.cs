﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EldewritoTickrateChanger
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            if (Environment.OSVersion.Version.Major == 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}