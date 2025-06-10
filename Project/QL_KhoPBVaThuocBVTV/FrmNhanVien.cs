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
    public partial class FrmNhanVien : Form
    {
        QL_KHOPhanBonThuocBVTVEntities qL_KHOPhanBonThuocBVTVEntities = new QL_KHOPhanBonThuocBVTVEntities();
        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void pcbAvatar_Click(object sender, EventArgs e)
        {

        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
        //        Title = "Chọn ảnh đại diện"
        //    };

        //    if (openFileDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        // Hiển thị ảnh lên PictureBox
        //        pcbAvatar.Image = Image.FromFile(openFileDialog.FileName);

        //        // Đọc ảnh thành byte[]
        //        byte[] imageBytes;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            pcbAvatar.Image.Save(ms, pcbAvatar.Image.RawFormat); // giữ đúng định dạng gốc
        //            imageBytes = ms.ToArray();
        //        }

        //        // Lưu vào cơ sở dữ liệu
        //        if (int.TryParse(MaND, out int maNguoiDung))
        //        {
        //            using (var db = new QL_KHOPhanBonThuocBVTVEntities())
        //            {
        //                var nguoiDung = db.NguoiDungs.FirstOrDefault(nd => nd.MaND == maNguoiDung);
        //                if (nguoiDung != null)
        //                {
        //                    nguoiDung.AnhDaiDien = imageBytes;
        //                    db.SaveChanges();
        //                    MessageBox.Show("Cập nhật ảnh đại diện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Mã người dùng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        }
    }
}
