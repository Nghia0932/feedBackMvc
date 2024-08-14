using System;
using System.ComponentModel.DataAnnotations;

namespace feedBackMvc.Models
{
    public class Surrvey
{
    [Key]
    public int Id { get; set; }

    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // Use UTC time
}

}
