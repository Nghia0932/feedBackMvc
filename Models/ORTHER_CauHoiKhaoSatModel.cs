using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedBackMvc.Models
{
    public class ORTHER_CauHoiKhaoSat
    {
        [Key]
        public int IdORTHER_CauHoiKhaoSat { get; set; }

        [Required]
        public string? TieuDeCauHoi { get; set; }

        [Required]
        public string? CauHoi { get; set; }

        public int IdORTHER_NhomCauHoiKhaoSat { get; set; }

        [ForeignKey("IdORTHER_NhomCauHoiKhaoSat")]
        public ORTHER_NhomCauHoiKhaoSat? NhomCauHoiKhaoSat { get; set; }
    }
}
