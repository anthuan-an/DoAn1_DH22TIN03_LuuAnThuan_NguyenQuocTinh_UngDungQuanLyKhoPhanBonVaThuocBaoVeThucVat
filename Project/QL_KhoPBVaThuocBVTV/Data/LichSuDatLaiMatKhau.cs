//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QL_KhoPBVaThuocBVTV.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class LichSuDatLaiMatKhau
    {
        public int MaLichSu { get; set; }
        public Nullable<int> MaND { get; set; }
        public Nullable<int> MaYeuCau { get; set; }
        public Nullable<System.DateTime> ThoiGian { get; set; }
        public string LoaiThaoTac { get; set; }
        public string ChiTiet { get; set; }
        public Nullable<int> NguoiThucHien { get; set; }
    
        public virtual YeuCauDatLaiMatKhau YeuCauDatLaiMatKhau { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual NguoiDung NguoiDung1 { get; set; }
    }
}
