// Animal.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pet_store_noamcelermajer.Models
{
    public class Animal
    {
        [Key]
        public int AnimalId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use alphabets and spaces only please")]
        public string? Name { get; set; }

        [Range(0, 50, ErrorMessage = "Age must be between 0 and 50.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string? PictureName { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }    

        [NotMapped]
        public int CommentCount { get; set; }
    }
}
