using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Comment
    {

        public int Id { get; set; }

        [ForeignKey("PostId")]
        [Required]
        public int PostId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public int UserId { get; set; }
        public int AuthorPostId { get; set; }
        public string Text { get; set; }

        // 0 - pending, 1 - accepted
        public int Status { get; set; }
        public DateTime Timestamp { get; set; }
        public Comment()
        {
            this.Timestamp = DateTime.UtcNow;
        }


    }
}
