using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class Friendship
    {
        public int Id { get; set; }
        public virtual User User1 { get; set; }

        [ForeignKey(nameof(User1)), Column(Order = 0)]
        public int User1Id { get; set; }

        public virtual User User2 { get; set; }

        [ForeignKey(nameof(User2)), Column(Order = 1)]
        public int User2Id { get; set; }
    }
}
