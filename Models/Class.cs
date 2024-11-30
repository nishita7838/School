using System.ComponentModel.DataAnnotations;

namespace Students.Models
{
    public class Class
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
