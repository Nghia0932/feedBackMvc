using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace feedBackMvc.Models
{
    public class ORTHER_NhomCauHoiKhaoSat
    {
        [Key]
        public int IdORTHER_NhomCauHoiKhaoSat { get; set; }

        [Required]
        [StringLength(5)]
        public string? TieuDe { get; set; }

        [Required]
        public string? NoiDung { get; set; }

        public int idAdmin { get; set; }

        [ForeignKey("idAdmin")]
        public Admins? Admins { get; set; }

        // Navigation property for related IN_CauHoiKhaoSat
        public ICollection<ORTHER_CauHoiKhaoSat>? CauHoiKhaoSats { get; set; }
    }
}
