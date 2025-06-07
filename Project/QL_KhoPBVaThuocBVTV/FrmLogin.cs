using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QL_KhoPBVaThuocBVTV.Data;

namespace QL_KhoPBVaThuocBVTV
{
    public partial class FrmLogin : Form
    {
        QL_KHOPhanBonThuocBVTVEntities QL_KHOPhanBonThuocBVTVEntities = new QL_KHOPhanBonThuocBVTVEntities();
        private int failedLoginCount = 0;
        private bool isPasswordShown = false;
        private bool isMouseHoldingEye = false;
        public bool IsLoginSuccess { get; private set; } = false;
        public string MaND { get; private set; }
        public string PhanQuyen { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.FormClosing += new FormClosingEventHandler(this.FrmLogin_FormClosing);
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            btnDangNhap.FlatStyle = FlatStyle.Flat;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnDangNhap.FlatAppearance.BorderSize = 0;
            btnThoat.FlatAppearance.BorderSize = 0;
            txtMatKhau.UseSystemPasswordChar = true;
            if (lblQuenMK != null)
            {
                lblQuenMK.Visible = false;
            }
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsLoginSuccess)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Environment.Exit(1);
                }
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DangNhap();
        }
        void DangNhap()
        {
            lblThongBao.Text = "Đang Đăng Nhập.....";

            var DangNhap = QL_KHOPhanBonThuocBVTVEntities.DangNhaps
                .Where(x => x.TaiKhoan == txtTenTaiKhoan.Text && x.MatKhau == txtMatKhau.Text)
                .FirstOrDefault();
            if (DangNhap != null)
            {
                if (DangNhap.TrangThai == true)
                {
                    MaND = DangNhap.MaND.Value.ToString();
                    PhanQuyen = DangNhap.PhanQuyen;
                    lblThongBao.Text = "Đăng Nhập Thành Công";
                    IsLoginSuccess = true;
                    this.Close();
                }
                else
                {
                    lblThongBao.Text = "Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên.";
                }
            }
            else
            {
                lblThongBao.Text = "Tài Khoản hoặc mật khẩu không đúng.";
                failedLoginCount++;
            }

            if (failedLoginCount >= 2 && lblQuenMK != null)
            {
                lblQuenMK.Visible = true;
                lblQuenMK.Text = "Quên Mật Khẩu ?";
            }
        }

        private void lblQuenMK_Click(object sender, EventArgs e)
        {
            using (FrmQuenMK frmQuenMK = new FrmQuenMK())
            {
                this.Hide();
                frmQuenMK.ShowDialog();
                this.Show();
            }
        }

        //private void lblDangKy_Click(object sender, EventArgs e)
        //{
        //    using (FrmDangKy frmDangKy = new FrmDangKy())
        //    {
        //        this.Hide();
        //        frmDangKy.ShowDialog();
        //        this.Show();
        //    }
        //}

        private void txtTenTaiKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtMatKhau.Focus();
            }
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DangNhap();
            }
        }

        private void btnHienPass_MouseUp(object sender, MouseEventArgs e)
        {

            isMouseHoldingEye = false;

            if (!isPasswordShown)
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
        }

        private void btnHienPass_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseHoldingEye = true;
            txtMatKhau.UseSystemPasswordChar = false;
        }

        private void btnHienPass_DoubleClick(object sender, EventArgs e)
        {
            isPasswordShown = !isPasswordShown;

            if (!isMouseHoldingEye)
            {
                txtMatKhau.UseSystemPasswordChar = !isPasswordShown;
            }
        }
    }
}

