using System.Collections.Generic;


namespace feedBackMvc.Models
{
    public class QL_DanhSachDanhGiaViewModel
    {
        public List<IN_ThongTinNguoiKhaoSat>? IN_TTNguoiKhaoSat { get; set; }
        public List<IN_MauKhaoSat>? IN_MauKhaoSatList { get; set; }

        public List<OUT_ThongTinNguoiKhaoSat>? OUT_TTNguoiKhaoSat { get; set; }
        public List<OUT_MauKhaoSat>? OUT_MauKhaoSatList { get; set; }

        public class IN_ThongTinNguoiKhaoSat
        {

            public DateTime NgayKhaoSat { get; set; }         // Survey date (NgayDienPhieu)
            public string? SoDienThoai { get; set; }           // Phone number
            // Department name (TenKhoa)
            public int? PhanTramMongDoi { get; set; }
            public string? QuayLaiVaGioiThieu { get; set; }
            public string? YKienKhac { get; set; }
            public string? Ten_IN_MauKhaoSat { get; set; }
            public int? IdIN_ThongTinNguoiBenh { get; set; }
            public int? IdIN_MauKhaoSat { get; set; }
        }
        public class OUT_ThongTinNguoiKhaoSat
        {

            public DateTime NgayKhaoSat { get; set; }         // Survey date (NgayDienPhieu)
            public string? SoDienThoai { get; set; }           // Phone number
            public int? PhanTramMongDoi { get; set; }
            public string? QuayLaiVaGioiThieu { get; set; }
            public string? YKienKhac { get; set; }
            public string? Ten_OUT_MauKhaoSat { get; set; }
            public int? IdOUT_ThongTinNguoiBenh { get; set; }
            public int? IdOUT_MauKhaoSat { get; set; }    // Department name (TenKhoa)
        }
    }
}
