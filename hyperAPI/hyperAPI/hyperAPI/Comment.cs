namespace hyperAPI
{
    public class Comment
    {

        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public Comment()
        {
            this.Timestamp = DateTime.UtcNow;
        }


    }
}
