using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class UserPR
    {
        public int Id { get; set; }

        [ForeignKey("PrId")]
        public PR Pr { get; set; }

        [Required]
        public int PrId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public int UserId { get; set; }

        public double Weight { get; set; }
        
    }
}
