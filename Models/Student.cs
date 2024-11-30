using System.ComponentModel.DataAnnotations;

namespace Students.Models
{
    public class Student
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage ="PhoneNumber must be 10 digits.")]
        public string? PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string? EmailId { get; set; }
        public ICollection<int> ClassIds { get; set; }


    }
}
