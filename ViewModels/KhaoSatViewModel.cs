namespace feedBackMvc.Models
{
    public class KhaoSatViewModel
    {
        public List<IN_MauKhaoSat>? IN_MauKhaoSatList { get; set; }
        public List<OUT_MauKhaoSat>? OUT_MauKhaoSatList { get; set; }
        public List<ORTHER_MauKhaoSat>? ORTHER_MauKhaoSatList { get; set; }
        public Dictionary<int, int>? CountSurvey_IN_MauKhaoSat { get; set; }
        public Dictionary<int, int>? CountSurvey_OUT_MauKhaoSat { get; set; }
        public Dictionary<int, int>? CountSurvey_ORTHER_MauKhaoSat { get; set; }
    }
}
