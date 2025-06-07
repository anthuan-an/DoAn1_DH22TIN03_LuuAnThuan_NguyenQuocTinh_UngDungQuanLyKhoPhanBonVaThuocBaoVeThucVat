using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using QL_KhoPBVaThuocBVTV.Data;

namespace QL_KhoPBVaThuocBVTV
{
    public partial class FrmQuenMK : Form
    {
        QL_KHOPhanBonThuocBVTVEntities QL_KHOPhanBonThuocBVTVEntities = new QL_KHOPhanBonThuocBVTVEntities();
        Random random = new Random();
        public FrmQuenMK()
        {
            InitializeComponent();
        }
        private void FrmQuenMK_Load(object sender, EventArgs e)
        {
            MenuXacNhan.Visible = false;
            lblTime.Visible = false;
            dtpNgaySinh.MaxDate = DateTime.Today;
        }
        private int countdown = 1800;

        private int OTP = -1;
        private void btnMaOTP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập email.");
                return;
            }
            else if (!txtEmail.Text.Trim().Contains("@") || !txtEmail.Text.Trim().Contains("."))
            {
                MessageBox.Show("Vui lòng nhập email hợp lệ.");
                return;
            }
            else if (QL_KHOPhanBonThuocBVTVEntities.NguoiDungs.FirstOrDefault(x => x.Email.Trim().ToLower() == txtEmail.Text.Trim().ToLower()) == null)
            {
                MessageBox.Show("Email không tồn tại trong hệ thống.");
                return;
            }
            else
            {
                if (timerOTP != null && timerOTP.Enabled)
                {
                    MessageBox.Show("Bạn đã yêu cầu mã OTP. Vui lòng đợi trước khi yêu cầu lại.");
                    return;
                }
            }
            OTP = random.Next(100000, 999999);

            var fromAddress = new MailAddress("quanlyphanbonthuoctrusau@gmail.com", "QL Kho PB & Thuốc BVTV");
            var toAddress = new MailAddress(txtEmail.Text.Trim());
            const string fromPassword = "dkouigpxwkvkhjck";
            const string subject = "Xác minh cấp lại mật khẩu";

            string body = $@"
            <div style='font-family: Arial; color: #333;'>
                <h2 style='color: #2E86C1;'>Yêu cầu cấp lại mật khẩu</h2>
                <p>Xin chào,</p>
                <p>Bạn vừa yêu cầu cấp lại mật khẩu cho tài khoản trên hệ thống <b>Quản lý kho phân bón & thuốc BVTV</b>.</p>
                <p style='margin-top:10px;'>
                    <b>Mã OTP của bạn là:</b> 
                    <span style='font-size: 20px; color: red; font-weight: bold;'>{OTP}</span>
                </p>
                <p style='color: red; margin-top:10px;'>
                    ⚠️ Vui lòng <b>KHÔNG chia sẻ</b> mã này với bất kỳ ai vì lý do bảo mật.
                </p>
                <p>Trân trọng,<br/>Hệ thống QL Kho</p>
            </div>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    smtp.Send(message);
                    MessageBox.Show("Mã OTP đã được gửi đến email của bạn.");

                    countdown = 1800;
                    lblTime.Text = $"{countdown}";

                    if (timerOTP == null)
                    {
                        timerOTP = new Timer();
                        timerOTP.Interval = 100000;
                        timerOTP.Tick += timerOTP_Tick;
                    }

                    timerOTP.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi gửi email: " + ex.Message);
                }
            }
        }
        private void timerOTP_Tick(object sender, EventArgs e)
        {
            countdown--;
            lblTime.Text = $"{countdown}";
            lblTime.Visible = true;

            if (countdown <= 0)
            {
                timerOTP.Stop();
                OTP = -1;
                lblTime.Visible = false;
            }
        }

        private void btnCapMatKhau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập email.");
                return;
            }

            if (string.IsNullOrEmpty(txtMaOTP.Text.Trim()))
            {
                MessageBox.Show("Vui lòng nhập mã OTP.");
                return;
            }

            if (txtMaOTP.Text.Trim() != OTP.ToString())
            {
                MessageBox.Show("Mã OTP không chính xác. Vui lòng thử lại.");
                return;
            }
            DateTime ngaySinh = dtpNgaySinh.Value.Date;

            var user = QL_KHOPhanBonThuocBVTVEntities.NguoiDungs
             .AsEnumerable()
             .FirstOrDefault(x =>
                 x.Email.Trim().ToLower() == txtEmail.Text.Trim().ToLower() &&
                 x.HoTen.Trim().ToLower() == txtHoTen.Text.Trim().ToLower() &&
                 x.SoCCCD.Trim() == txtSoCCCD.Text.Trim() &&
                 x.SoDienThoai.Trim() == txtSDT.Text.Trim() &&
                 x.NgaySinh.Date == ngaySinh
             );
            if (user == null)
            {
                MessageBox.Show("Thông tin bạn nhập không đúng hoặc không tồn tại trong hệ thống.");
                return;
            }
            else if (user.VaiTro == "Admin")
            {
                MessageBox.Show("Tài khoản quản trị (Admin) là tài khoản đặc biệt và không thể cấp lại mật khẩu qua chức năng này.\nVui lòng liên hệ quản trị viên hệ thống qua SDT:0911118386.");
                return;

            }
            else if (user.TrangThai == false)
            {
                MessageBox.Show("Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.");
                return;
            }
            else if (QL_KHOPhanBonThuocBVTVEntities.YeuCauDatLaiMatKhaus.Any(x => x.MaND == user.MaND && x.TrangThai == "Chờ xác nhận"))
            {
                MessageBox.Show("Bạn đã có yêu cầu cấp lại mật khẩu đang chờ xác nhận.\n Vui lòng đợi hoặc liên hệ quản trị viên qua SDT:0911118386.");
                return;
            }
            else
            {
                MessageBox.Show("Thông tin xác thực thành công !. \n Hãy tạo mật khẩu mới !");
                timerOTP.Stop();
                lblTime.Visible = false;

                //Thêm đoạn này ngay tại đây
                var ycMoi = new YeuCauDatLaiMatKhau
                {
                    MaND = user.MaND,
                    NgayYeuCau = DateTime.Now,
                    TrangThai = "Chờ xác nhận",
                    MatKhauMoi = null
                };

                QL_KHOPhanBonThuocBVTVEntities.YeuCauDatLaiMatKhaus.Add(ycMoi);
                QL_KHOPhanBonThuocBVTVEntities.SaveChanges();
            }

            MenuXacNhan.Visible = true;
            if (OTP == -1)
            {
                MessageBox.Show("Mã OTP đã hết hạn");
            }

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (txtMatKhau.Text != txtNhapLai.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.");
                return;
            }

            var y = QL_KHOPhanBonThuocBVTVEntities.NguoiDungs.FirstOrDefault(x => x.Email == txtEmail.Text.Trim());
            if (y != null)
            {
                var yc = QL_KHOPhanBonThuocBVTVEntities.YeuCauDatLaiMatKhaus.FirstOrDefault(x => x.MaND == y.MaND && x.TrangThai == "Chờ xác nhận");
                yc.MatKhauMoi = txtMatKhau.Text.Trim();
                QL_KHOPhanBonThuocBVTVEntities.SaveChanges();
                MessageBox.Show(
                "Chờ xác nhận Mật Khẩu!\nKhi mật khẩu được xác nhận sẽ được thông báo qua email.",
                "Thông Báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );
                txtEmail.Text = "";
                txtHoTen.Text = "";
                txtSoCCCD.Text = "";
                txtSDT.Text = "";
                txtMaOTP.Text = "";
                txtMatKhau.Text = "";
                txtNhapLai.Text = "";
                dtpNgaySinh.Value = DateTime.Today;

                MenuXacNhan.Visible = false;

                OTP = -1;
                if (timerOTP != null) timerOTP.Stop();
                lblTime.Visible = false;
            }
        }
    }
}
