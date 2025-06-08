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
        private string PhanQuyen;
        private Form currentChildForm;

        public FrmQuanLyTaiKhoan(string maND, string phanQuyen)
        {
            InitializeComponent();
            this.maND = maND;
            PhanQuyen = phanQuyen;
        }
        private void FrmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            ApDungPhanQuyen();
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
            lblThongBao.Text = "Quản Lý Tài Khoản";
            OpenChildForm(new FrmTaiKhoan());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            lblThongBao.Text = "Quản Lý Nhân Viên";
            OpenChildForm(new FrmNhanVien());
        }

        private void btnCapLai_Click(object sender, EventArgs e)
        {
            lblThongBao.Text = "Đặt Lại Mật Khẩu";
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

        private void btnKhachHang_Click(object sender, EventArgs e)
        {
            lblThongBao.Text = "Quản Lý Khách Hàng";
            OpenChildForm(new FrmKhachHang());
        }
        private void ApDungPhanQuyen()
        {
            if(PhanQuyen == "Nhân viên")
            {
                btnCapLai.Visible = false;
                btnTaiKhoan.Visible = false;
                btnNhanVien.Visible = false;
                OpenChildForm(new FrmKhachHang());
                lblThongBao.Text = "Quản Lý Khách Hàng";
            }
            lblThongBao.Text = "Quản Lý Nhân Viên";
            OpenChildForm(new FrmNhanVien());
        }


    }
}
