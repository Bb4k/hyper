using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Message
    {

        public int Id { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public int UserFromId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public int UserToId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public Message()
        {
            this.Timestamp = DateTime.UtcNow;
        }


    }
}
