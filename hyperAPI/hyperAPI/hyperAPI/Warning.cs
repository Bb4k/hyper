using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Warning
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey("PostId")]
        [Required]
        public int PostId { get; set; }
        public int CommentId { get; set; }
    }
}
