using System.Collections.Generic;


namespace feedBackMvc.Models
{
    public class QL_ThongTinNguoiKhaoSatViewModel
    {
        public List<IN_ThongTinNguoiKhaoSat>? IN_TTNguoiKhaoSat { get; set; }
        public List<IN_MauKhaoSat>? IN_MauKhaoSatList { get; set; }

        public List<OUT_ThongTinNguoiKhaoSat>? OUT_TTNguoiKhaoSat { get; set; }
        public List<OUT_MauKhaoSat>? OUT_MauKhaoSatList { get; set; }
        public class IN_ThongTinNguoiKhaoSat
        {
            public string? TenBenhVien { get; set; }           // Hospital name
            public DateTime NgayKhaoSat { get; set; }         // Survey date (NgayDienPhieu)
            public string? SoDienThoai { get; set; }           // Phone number
            public string? NguoiTraLoi { get; set; }           // Person responding to the survey
            public string? GioiTinh { get; set; }              // Gender
            public int? Tuoi { get; set; }                    // Age
            public int? SoNgayNamVien { get; set; }           // Number of days in hospital
            public bool? CoSuDungBHYT { get; set; }           // Whether health insurance was used
            public string? TenKhoa { get; set; }               // Department name (TenKhoa)
            public string? Ten_IN_MauKhaoSat { get; set; }

        }
        public class OUT_ThongTinNguoiKhaoSat
        {
            public string? TenBenhVien { get; set; }           // Hospital name
            public DateTime NgayKhaoSat { get; set; }         // Survey date (NgayDienPhieu)
            public string? SoDienThoai { get; set; }           // Phone number
            public string? NguoiTraLoi { get; set; }           // Person responding to the survey
            public string? GioiTinh { get; set; }              // Gender
            public int? Tuoi { get; set; }                    // Age
            public int? KhoangCach { get; set; }           // Number of days in hospital
            public bool? CoSuDungBHYT { get; set; }           // Whether health insurance was used
            public string? Ten_OUT_MauKhaoSat { get; set; }
            // Department name (TenKhoa)
        }


    }
}
