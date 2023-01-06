using System.ComponentModel.DataAnnotations.Schema;

namespace hyperAPI
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Height { get; set; } 
        public double Weight { get; set; }
        public string Picture { get; set; }
        public ICollection<UserPR> UserPRs { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Friendship.User1))]
        public ICollection<Friendship> Friendships1 { get; set; }

        [InverseProperty(nameof(Friendship.User2))]
        public ICollection<Friendship> Friendships2 { get; set; }


    }
}
