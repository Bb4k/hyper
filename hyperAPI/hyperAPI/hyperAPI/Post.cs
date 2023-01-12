using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Post
    {
        public int Id { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public int UserId { get; set; }
        public double Weight { get; set; }

        [ForeignKey("PrId")]
        [Required]
        public int PrId { get; set; }
        public string Media1 { get; set; }
        public string Media2 { get; set; }
        public string Media3 { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; } = 0;
        public DateTime Timestamp { get; set; }
        public Post()
        {
            this.Timestamp = DateTime.UtcNow;
        }

    }
}
