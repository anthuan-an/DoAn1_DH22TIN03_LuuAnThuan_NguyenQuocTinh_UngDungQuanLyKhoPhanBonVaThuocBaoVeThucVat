﻿using QL_KhoPBVaThuocBVTV.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoPBVaThuocBVTV
{
    public partial class FrmMain : Form
    {
        string PhanQuyen = string.Empty;
        public string MaND { get; set; }
        private Form currentChildForm;
        bool SidebarExpanded = false;
        Panel currentMenuPanel = null;
        bool currentMenuExpanded = false;
        private bool isLoggingOut = false;
        Panel nextMenuPanel = null;


        public FrmMain()
        {
            InitializeComponent();
        }
        public FrmMain(string maND, string phanQuyen)
        {
            InitializeComponent();
            MaND = maND;
            PhanQuyen = phanQuyen;
        }

        private string LayTenNguoiDung(string maND)
        {
            int maNDInt = int.Parse(maND);

            using (var context = new QL_KHOPhanBonThuocBVTVEntities())
            {
                return context.NguoiDungs
                              .Where(nd => nd.MaND == maNDInt)
                              .Select(nd => nd.HoTen)
                              .FirstOrDefault() ?? "";
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            int maNguoiDung = int.Parse(MaND);
            string tenNguoiDung = LayTenNguoiDung(MaND);
            pnlBanTB.Visible = false;
            FrmTrangChuPhu trangChuPhu = new FrmTrangChuPhu
            {
                TenNguoiDung = tenNguoiDung,
                Quyen = PhanQuyen,
                MaND = MaND,

            };
            ApDungPhanQuyen();
            OpenChildForm(trangChuPhu);
            CapNhatThongBao();
        }
        private void ApDungPhanQuyen()
        {
            switch (PhanQuyen)
            {
                case "Admin":
                    break;

                case "Nhân viên":
                    //sản phẩm
                    MenuKho.Visible = false;
                    //nhap hang
                    MenuHangTon.Visible = false;
                    MenuBaoCaoNhapKho.Visible = false;
                    // xuat hang
                    MenuChuyenKho.Visible = false;
                    MenuBaoCaoXuatKho.Visible = false;
                    MenuXuatKho.Visible = false;
                    // lịch sữ
                    MenuLichSuGia.Visible = false;

                    pnlThongBao.Visible = false;
                    break;

                case "Thủ kho":
                    pnlThongBao.Visible = false;
                    MenuQuanLy.Visible = false;
                    MenuLichSuGia.Visible = false;
                    break;

                default:
                    MessageBox.Show("Phân quyền không hợp lệ. Vui lòng liên hệ quản trị viên.", "Lỗi phân quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FrmLogin frmLogin = new FrmLogin();
                    frmLogin.Show();
                    this.Hide();
                    break;

            }
        }

        private void CapNhatThongBao()
        {
            using (var context = new QL_KHOPhanBonThuocBVTVEntities())
            {
                var dsYeuCau = (from yc in context.YeuCauDatLaiMatKhaus
                                join nd in context.NguoiDungs on yc.MaND equals nd.MaND
                                where yc.TrangThai == "Chờ xác nhận"
                                select new
                                {
                                    yc.MaYeuCau,
                                    yc.MaND,
                                    nd.HoTen,
                                    yc.NgayYeuCau
                                }).ToList();

                lblThongBao.Text = dsYeuCau.Count.ToString();
                lblThongBao.Visible = dsYeuCau.Count > 0;

                // Clear các control cũ
                pnlBanTB.Controls.Clear();

                int y = 10;
                foreach (var item in dsYeuCau)
                {
                    Panel panel = new Panel();
                    panel.Size = new Size(pnlBanTB.Width - 20, 40);
                    panel.Location = new Point(10, y);
                    panel.BackColor = Color.White;
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Cursor = Cursors.Hand;

                    Label lbl = new Label();
                    lbl.Text = $"Nhân viên {item.HoTen} xin cấp lại mật khẩu.";
                    lbl.Dock = DockStyle.Fill;
                    lbl.TextAlign = ContentAlignment.MiddleLeft;
                    lbl.Padding = new Padding(5);
                    lbl.Tag = item.MaYeuCau;
                    lbl.Cursor = Cursors.Hand;

                    // Xử lý hover (MouseEnter / MouseLeave)
                    panel.MouseEnter += (s, e) => { panel.BackColor = Color.LightGray; };
                    panel.MouseLeave += (s, e) => { panel.BackColor = Color.White; };
                    lbl.MouseEnter += (s, e) => { panel.BackColor = Color.LightGray; };
                    lbl.MouseLeave += (s, e) => { panel.BackColor = Color.White; };

                    // Xử lý click để mở form xử lý yêu cầu
                    lbl.Click += (s, e) =>
                    {
                        int maYeuCau = (int)((Label)s).Tag;
                        FrmQuanLyTaiKhoan frmQLTK = new FrmQuanLyTaiKhoan(MaND, PhanQuyen);

                        // Gán sự kiện load cho FrmQuanLyTaiKhoan để chỉ gọi MoFormCon sau khi load xong
                        frmQLTK.Load += (sender2, e2) =>
                        {
                            FrmDatLaiMK frmDatLaiMK = new FrmDatLaiMK(maYeuCau.ToString());
                            frmQLTK.MoFormConTrongPanel(frmDatLaiMK);
                        };

                        OpenChildForm(frmQLTK);
                        pnlBanTB.Visible = false;
                    };

                    panel.Controls.Add(lbl);
                    pnlBanTB.Controls.Add(panel);

                    y += panel.Height + 10;
                }
            }
        }
        private void LblYeuCau_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null && lbl.Tag != null)
            {
                string maND = lbl.Tag.ToString();
                FrmDatLaiMK frm = new FrmDatLaiMK(maND);
                OpenChildForm(frm);
                pnlBanTB.Visible = false;
            }
        }
        private void btnThongBao_Click(object sender, EventArgs e)
        {
            if (pnlBanTB.Visible)
            {
                pnlBanTB.Visible = false;
            }
            else
            {
                CapNhatThongBao();
                pnlBanTB.Visible = true;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!isLoggingOut)
             {
                    DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                    {
                        e.Cancel = true;
                }
                else
                {
                    // Thoát hoàn toàn ứng dụng
                    Environment.Exit(0);
                }
            }
        }
        private int GetMenuHeight(Panel menu)
        {
            int height = 47;

            foreach (Control c in menu.Controls)
            {
                if (c.Visible)
                {
                    height += c.Height;
                }
            }

            return height;
        }


        private void MenuTransition_Tick(object sender, EventArgs e)
        {
            if (currentMenuPanel == null) return;

            int targetHeight = GetMenuHeight(currentMenuPanel);

            if (currentMenuExpanded)
            {
                currentMenuPanel.Height -= 10;
                if (currentMenuPanel.Height <= 57)
                {
                    currentMenuPanel.Height = 57;
                    MenuTransition.Stop();
                    currentMenuExpanded = false;
                    currentMenuPanel = null;

                    if (nextMenuPanel != null)
                    {
                        currentMenuPanel = nextMenuPanel;
                        currentMenuExpanded = false;
                        nextMenuPanel = null;
                        MenuTransition.Start();
                    }
                }
            }
            else
            {
                currentMenuPanel.Height += 10;
                if (currentMenuPanel.Height >= targetHeight)
                {
                    currentMenuPanel.Height = targetHeight;
                    MenuTransition.Stop();
                    currentMenuExpanded = true;
                }
            }
        }

        private void ToggleMenu(Panel menuPanel)
        {
            if (currentMenuPanel == null)
            {
                currentMenuPanel = menuPanel;
                currentMenuExpanded = false;
                MenuTransition.Start();
            }
            else if (currentMenuPanel == menuPanel)
            {
                MenuTransition.Start();
            }
            else
            {
                nextMenuPanel = menuPanel;
                MenuTransition.Start();
            }
        }

        private void SidebarTransition_Tick(object sender, EventArgs e)
        {
            if (SidebarExpanded == false)
            {
                Sidebar.Width -= 15;
                if (Sidebar.Width <= 75)
                {
                    SidebarTransition.Stop();
                    SidebarExpanded = true;
                }
            }
            else
            {
                Sidebar.Width += 15;
                if (Sidebar.Width >= 247)
                {
                    SidebarTransition.Stop();
                    SidebarExpanded = false;
                }
            }
        }

        private void picBSidebar_Click(object sender, EventArgs e)
        {
            SidebarTransition.Start();
        }
        private void BtnSanPham_Click(object sender, EventArgs e)
        {
            ToggleMenu(MenuSanPham);
        }

        private void btnNhaphang_Click(object sender, EventArgs e)
        {
            ToggleMenu(MenuNhapHang);
        }

        private void btnXuatHang_Click(object sender, EventArgs e)
        {
            ToggleMenu(MenuXuatHang);
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            ToggleMenu(MenuBaoCao);
        }

        private void btnLichSu_Click(object sender, EventArgs e)
        {
            ToggleMenu(MenuLichSu);
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

            panelMain.Controls.Clear();
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }
        // trang chủ
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            string tenNguoiDung = LayTenNguoiDung(MaND);
            FrmTrangChuPhu trangChuPhu = new FrmTrangChuPhu
            {
                TenNguoiDung = tenNguoiDung,
                Quyen = PhanQuyen,
                MaND = MaND
            };
            OpenChildForm(trangChuPhu);
        }


        // ql tài Khoản
        private void btnQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmQuanLyTaiKhoan(MaND,PhanQuyen));
        }

        // cài đặt
        private void btnCaiDat_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmCaiDat());
        }
        // giới thiệu
        private void btnGioiThieu_Click(object sender, EventArgs e)
        {

        }
        //Dang Xuất
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            isLoggingOut = true;
            this.Close();
        }

        //
        private void lblTen_Click(object sender, EventArgs e)
        {
            string tenNguoiDung = LayTenNguoiDung(MaND);
            FrmTrangChuPhu trangChuPhu = new FrmTrangChuPhu
            {
                TenNguoiDung = tenNguoiDung,
                Quyen = PhanQuyen,
                MaND = MaND
            };
            OpenChildForm(trangChuPhu);
        }
        // Sản Phẩm
        private void btnHangHoa_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmHangHoa(MaND));
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmDanhMuc());
        }

        private void btnNhaSX_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmNhaSanXuat());
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FrmKho());
        }


        // Nhập Hàng
        private void btnHangTon_Click(object sender, EventArgs e)
        {

        }

        private void btnHangMoi_Click(object sender, EventArgs e)
        {

        }

        private void btnHopDong_Click(object sender, EventArgs e)
        {

        }

        private void btnBaoCaoNhapKho_Click(object sender, EventArgs e)
        {

        }
        // Xuất Hàng
        private void btnChuyenKho_Click(object sender, EventArgs e)
        {

        }

        private void btnPhieuXuatKho_Click(object sender, EventArgs e)
        {

        }

        private void btnChiTietXuatKho_Click(object sender, EventArgs e)
        {

        }

        private void btnBaoCaoXuatKho_Click(object sender, EventArgs e)
        {

        }
        // Báo Cáo
        private void btnBaoCaoNhapXuatTon_Click(object sender, EventArgs e)
        {

        }

        private void btnBaoCaoTonKho_Click(object sender, EventArgs e)
        {

        }

        private void btnBaoCaoNhapKhoTheoDoiTuong_Click(object sender, EventArgs e)
        {

        }

        private void btnBaoCaoXuatKhoTheoDoiTuong_Click(object sender, EventArgs e)
        {

        }
        //Lịch sử
        private void btnLichSuGia_Click(object sender, EventArgs e)
        {

        }

        private void btnLichSuDangNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnLichSuDatLaiMK_Click(object sender, EventArgs e)
        {

        }

        private void btnDanhGia_Click(object sender, EventArgs e)
        {

        }

       }
}
