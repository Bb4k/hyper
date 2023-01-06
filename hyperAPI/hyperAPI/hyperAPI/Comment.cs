using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Comment
    {

        public int Id { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int UserId { get; set; }

        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public Comment()
        {
            this.Timestamp = DateTime.UtcNow;
        }


    }
}
