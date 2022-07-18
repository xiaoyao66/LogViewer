using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            if (!Debugger.IsAttached) {
                MessageBox.Show("Debug");
            }
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if true
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                Debug.WriteLine("DLL load:" + args.Name);
                string name = args.Name.ToLower();

                if (name.Contains("objectlistview"))
                    return System.Reflection.Assembly.Load(LogViewer.Properties.Resources.ObjectListView);

                if (name.Contains("utility"))
                    return System.Reflection.Assembly.Load(LogViewer.Properties.Resources.Utility);

                return null;
            };
#endif
            Application.Run(new FormMain());
        }
    }
}
