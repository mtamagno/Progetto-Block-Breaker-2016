using System;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
        }

        #endregion Private Methods
    }
}