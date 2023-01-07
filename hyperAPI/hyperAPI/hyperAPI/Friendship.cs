using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Friendship
    {
        public int Id { get; set; }

        [ForeignKey("User1Id"), Column(Order = 0)]
        [Required]
        public int User1Id { get; set; }

        [ForeignKey("User2Id"), Column(Order = 1)]
        [Required]
        public int User2Id { get; set; }

        public int Status { get; set; }
    }
}
