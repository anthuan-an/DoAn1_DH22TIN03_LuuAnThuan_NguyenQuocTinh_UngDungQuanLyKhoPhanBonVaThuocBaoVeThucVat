using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoPBVaThuocBVTV
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmLogin frmLogin = new FrmLogin();
            Application.Run(frmLogin);

            if (frmLogin.IsLoginSuccess)
            {
                Application.Run(new FrmMain(frmLogin.MaND, frmLogin.PhanQuyen));
            }
        }
    }
}