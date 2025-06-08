using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QL_KhoPBVaThuocBVTV
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool isExit = false;

            while (!isExit)
            {
                FrmLogin frmLogin = new FrmLogin();
                Application.Run(frmLogin);

                if (frmLogin.IsLoginSuccess)
                {
                    FrmMain frmMain = new FrmMain(frmLogin.MaND, frmLogin.PhanQuyen);
                    Application.Run(frmMain);

                }
                else
                {
                    isExit = true;
                }
            }
        }
    }
}
