// Category.cs
using System.ComponentModel.DataAnnotations;

namespace pet_store_noamcelermajer.Models
{
    public class Category
    {
        [Key]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }
    }
}
