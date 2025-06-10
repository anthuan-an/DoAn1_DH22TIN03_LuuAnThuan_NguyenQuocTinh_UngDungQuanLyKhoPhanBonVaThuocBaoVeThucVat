using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using QL_KhoPBVaThuocBVTV.Data;
using System.IO;
using System.Data.Common.CommandTrees.ExpressionBuilder;


namespace QL_KhoPBVaThuocBVTV
{
    public partial class FrmTrangChuPhu : Form
    {
        public string TenNguoiDung { get; set; }
        public string Quyen { get; set; }
        public string MaND { get; set; } // Giữ kiểu string để tiện truyền từ FrmMain
        private ImageZoom imageZoom;

        QL_KHOPhanBonThuocBVTVEntities QL_KHOPhanBonThuocBVTVEntities = new QL_KHOPhanBonThuocBVTVEntities();
        public FrmTrangChuPhu()
        {
            InitializeComponent();
        }

        private void FrmTrangChuPhu_Load(object sender, EventArgs e)
        {
            timerTime.Interval = 1000;
            timerTime.Start();
            lblChucVu.Text = Quyen + " :";
            lblTen.Text = TenNguoiDung;

            if (int.TryParse(MaND, out int maNguoiDung))
            {
                LoadAnhDaiDien(maNguoiDung);
            }
            else
            {
                MessageBox.Show("Mã người dùng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SapHetHang_Load();
            SapHetHH_Load();
            imageZoom = new ImageZoom();
            imageZoom.Attach(dgvDanhSachHH);
            imageZoom.Attach(dgvDanhSach);
            LoadDanhSachKhoHang(cbbKhoHang);
            LoadDanhSachKhoHang(cbbKhoHang1);
            cbbKhoHang.SelectedIndexChanged += cbbKhoHang_SelectedIndexChanged;
            cbbKhoHang1.SelectedIndexChanged += cbbKhoHang1_SelectedIndexChanged;
            if (cbbKhoHang.SelectedValue != null)
            {
                HienThiSoLuongNhap((int)cbbKhoHang.SelectedValue);
            }

            if (cbbKhoHang1.SelectedValue != null)
            {
                HienThiSoLuongXuat((int)cbbKhoHang1.SelectedValue);
            }
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            var culture = new CultureInfo("vi-VN");
            DateTime now = DateTime.Now;
            lblNgayVN.Text = now.ToString("dddd, dd/MM/yyyy - HH:mm:ss", culture);
        }

        private void LoadAnhDaiDien(int maND)
        {
            using (var db = new QL_KHOPhanBonThuocBVTVEntities())
            {
                var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.MaND == maND);
                if (nguoiDung != null && nguoiDung.AnhDaiDien != null && nguoiDung.AnhDaiDien.Length > 0)
                {

                    try
                    {
                        using (var ms = new MemoryStream(nguoiDung.AnhDaiDien))
                        {
                            pcbAvatar.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        pcbAvatar.Image = Properties.Resources.icons8_account_32;
                    }
                }
                else
                {
                    pcbAvatar.Image = Properties.Resources.icons8_account_32; // Ảnh mặc định nếu không có ảnh
                }
            }
        }

        private void SapHetHang_Load()
        {
            using (var db = new QL_KHOPhanBonThuocBVTVEntities())
            {
                var query = from hh in db.SanPhams
                            join k in db.Khoes on hh.MaKho equals k.MaKho
                            where hh.SoLuongTon <= 10
                            select new
                            {
                                hh.MaSP,
                                hh.AnhSP,
                                hh.TenSP,
                                Kho = k.TenKho,
                                hh.LoaiSanPham.TenLoai,
                                hh.NgayTao,
                                hh.NhaCungCap.TenNCC,
                                hh.SoLuongTon
                            };

                var dataList = query.ToList();

                dgvDanhSach.Rows.Clear();

                foreach (var item in dataList)
                {
                    // Xử lý ảnh
                    Image img = Properties.Resources.icons8_fast_moving_consumer_goods_32; // Ảnh mặc định
                    if (item.AnhSP != null && item.AnhSP.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(item.AnhSP))
                        {
                            img = Image.FromStream(ms);
                        }
                    }

                    dgvDanhSach.Rows.Add(item.MaSP, img, item.TenSP, item.Kho, item.TenLoai, ((DateTime)item.NgayTao).ToString("dd/MM/yyyy"), item.TenNCC, item.SoLuongTon);
                }
            }
        }
        private void SapHetHH_Load()
        {
            using (var db = new QL_KHOPhanBonThuocBVTVEntities())
            {
                var today = DateTime.Today;

                // Truy vấn đơn giản không dùng phép tính DateTime
                var dataList = (from hh in db.SanPhams
                                join k in db.Khoes on hh.MaKho equals k.MaKho
                                where hh.HanSuDung != null
                                select new
                                {
                                    hh.MaSP,
                                    hh.AnhSP,
                                    hh.TenSP,
                                    hh.HanSuDung,
                                    Kho = k.TenKho
                                }).ToList();

                // Tính toán ngày còn lại sau khi lấy dữ liệu về
                var filteredList = dataList
                    .Where(item => (item.HanSuDung.Value - today).TotalDays >= 0 &&
                                   (item.HanSuDung.Value - today).TotalDays <= 15)
                    .ToList();

                dgvDanhSachHH.Rows.Clear();

                foreach (var item in filteredList)
                {
                    int soNgayConLai = (item.HanSuDung.Value - today).Days;

                    // Ảnh mặc định nếu không có ảnh trong DB
                    Image img = Properties.Resources.icons8_fast_moving_consumer_goods_32;
                    if (item.AnhSP != null && item.AnhSP.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(item.AnhSP))
                        {
                            img = Image.FromStream(ms);
                        }
                    }

                    dgvDanhSachHH.Rows.Add(
                        item.MaSP, img, item.TenSP, $"{item.HanSuDung?.ToString("dd")}\n{item.HanSuDung?.ToString("MM/yyyy")}", item.Kho, $"{soNgayConLai} ngày"
                    );
                }
            }
        }
        private void LoadDanhSachKhoHang(ComboBox comboBox)
        {
            comboBox.DataSource = QL_KHOPhanBonThuocBVTVEntities.Khoes.ToList();
            comboBox.DisplayMember = "TenKho";
            comboBox.ValueMember = "MaKho";
            comboBox.SelectedIndex = 0;
        }
        private void cbbKhoHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbKhoHang.SelectedValue != null)
            {
                int maKho = (int)cbbKhoHang.SelectedValue;
                HienThiSoLuongNhap(maKho);
            }
        }
        private void HienThiSoLuongNhap(int maKho)
        {
            DateTime now = DateTime.Now;
            int daysToSubtract = (int)now.DayOfWeek == 0 ? 6 : (int)now.DayOfWeek - 1;
            DateTime dauTuan = now.Date.AddDays(-daysToSubtract);
            DateTime dauThang = new DateTime(now.Year, now.Month, 1);

            using (var db = new QL_KHOPhanBonThuocBVTVEntities())
            {
                int tongNhapTuan = db.PhieuNhapKhoes
                    .Where(p => p.NgayNhap >= dauTuan && p.MaKho == maKho)
                    .SelectMany(p => p.ChiTietPhieuNhaps)
                    .Sum(ct => (int?)ct.SoLuong) ?? 0;

                int tongNhapThang = db.PhieuNhapKhoes
                    .Where(p => p.NgayNhap >= dauThang && p.MaKho == maKho)
                    .SelectMany(p => p.ChiTietPhieuNhaps)
                    .Sum(ct => (int?)ct.SoLuong) ?? 0;

                lblNhapTuan.Text = tongNhapTuan.ToString();
                lblNhapThang.Text = tongNhapThang.ToString();
            }
        }


        private void cbbKhoHang1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbKhoHang1.SelectedValue != null)
            {
                int maKho = (int)cbbKhoHang1.SelectedValue;
                HienThiSoLuongXuat(maKho);
            }
        }
        private void HienThiSoLuongXuat(int maKho)
        {
            DateTime now = DateTime.Now;
            int daysToSubtract = (int)now.DayOfWeek == 0 ? 6 : (int)now.DayOfWeek - 1;
            DateTime dauTuan = now.Date.AddDays(-daysToSubtract);
            DateTime dauThang = new DateTime(now.Year, now.Month, 1);

            using (var db = new QL_KHOPhanBonThuocBVTVEntities())
            {
                int tongXuatTuan = db.PhieuXuatKhoes
                    .Where(p => p.NgayXuat >= dauTuan && p.MaKho == maKho)
                    .SelectMany(p => p.ChiTietPhieuXuats)
                    .Sum(ct => (int?)ct.SoLuong) ?? 0;

                int tongXuatThang = db.PhieuXuatKhoes
                    .Where(p => p.NgayXuat >= dauThang && p.MaKho == maKho)
                    .SelectMany(p => p.ChiTietPhieuXuats)
                    .Sum(ct => (int?)ct.SoLuong) ?? 0;

                lblXuatTuan.Text = tongXuatTuan.ToString();
                lblXuatThang.Text = tongXuatThang.ToString();
            }
        }
    }

}
