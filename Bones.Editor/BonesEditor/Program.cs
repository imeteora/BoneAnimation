#region File Description
//-----------------------------------------------------------------------------
// Program.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Windows.Forms;
using BoneLibrary;
#endregion

namespace BoneEditor
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string path = null;
            string[] args = Environment.GetCommandLineArgs();
            foreach (var s in args)
            {
                if( s.Contains(SkeletonAsset.EXTENSION))
                {
                    path = s;
                    break;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main(path));
        }
    }
}
