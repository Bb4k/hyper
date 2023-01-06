namespace hyperAPI
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Media { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; } = 0;
        public ICollection<Comment> Comments { get; set; }
        public DateTime Timestamp { get; set; }
        public Post()
        {
            this.Timestamp = DateTime.UtcNow;
        }

    }
}
