namespace hyperAPI
{
    public class UserPR
    {
        public int Id { get; set; }
        public int PrId { get; set; }
        public PR Pr { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public double Weight { get; set; }
        
    }
}
