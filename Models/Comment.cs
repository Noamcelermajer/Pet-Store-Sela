// Comment.cs
using System.ComponentModel.DataAnnotations;

namespace pet_store_noamcelermajer.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        public string CommentText { get; set; }

        public int AnimalId { get; set; }
    }
}
