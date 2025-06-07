using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoPBVaThuocBVTV
{
    public partial class FrmQuanLyTaiKhoan : Form
    {
        private string maND;
        private Form currentChildForm;
        public FrmQuanLyTaiKhoan(string maND)
        {
            InitializeComponent();
            this.maND = maND;
        }

        private void btnHien_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = true;
            btnHien.Visible = false;
            btnAn.Visible = true;
            pnlMenu.Visible = false;
            pnlMenu.Width = 230;
            TrSTInOuter.ShowSync(pnlMenu);
        }

        private void btnAn_Click(object sender, EventArgs e)
        {
            pnlMenu.Visible = false;
            pnlMenu.Width = 70;
            btnHien.Visible = true;
            btnAn.Visible = false;
            TrSTInOuter.ShowSync(pnlMenu);
        }
        private void OpenChildForm(Form childForm)
        {
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }

            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlQuanLy.Controls.Clear();
            pnlQuanLy.Controls.Add(childForm);
            pnlQuanLy.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmTaiKhoan());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmNhanVien());
        }

        private void btnCapLai_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmDatLaiMK(maND));
        }
        public void MoFormConTrongPanel(Form form)
        {
            pnlQuanLy.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlQuanLy.Controls.Add(form);
            form.Show();
        }
    }
    //70, 713
    //281, 713
}
