namespace feedBackMvc.Models
{
    public class DashboardViewModel
    {
        public List<string>? IN_MangCauHoi { get; set; }
        public List<List<double>>? IN_DanhGiaTong { get; set; }
        public List<string>? OUT_MangCauHoi { get; set; }
        public List<List<double>>? OUT_DanhGiaTong { get; set; }

        // New properties
        public List<string>? IN_DanhSachTenKhoa { get; set; }
        public List<string>? OUT_DanhSachTenKhoa { get; set; }
        public List<int>? IN_PhanTramMongDoi { get; set; }
        public List<int>? OUT_PhanTramMongDoi { get; set; }
        public List<KhoaCountViewModel>? IN_KhoaCounts { get; set; }
        public List<KhoaCountViewModel>? OUT_KhoaCounts { get; set; }
        public List<PhanTramMongDoiViewModel>? IN_PhanTramMongDoiTheoKhoa { get; set; }
        public List<PhanTramMongDoiViewModel>? OUT_PhanTramMongDoiTheoKhoa { get; set; }


    }

    public class KhoaCountViewModel
    {
        public string? TenKhoa { get; set; }
        public int Count { get; set; }
    }
    public class PhanTramMongDoiViewModel
    {
        public string? TenKhoa { get; set; }
        public double PhanTramMongDoi { get; set; }
    }
}
