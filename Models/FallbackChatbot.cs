using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
namespace feedBackMvc.Models
{
    public class FallbackChatbot
    {
        [Key]
        public int id { get; set; }
        public string? CauHoiTruoc { get; set; }
        public string? CauHoi { get; set; }
        public string? Intent { get; set; }
        public DateOnly CreatedDate { get; set; }

    }


}