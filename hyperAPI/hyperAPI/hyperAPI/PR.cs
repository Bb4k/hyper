namespace hyperAPI
{
    public class PR
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ICollection<UserPR> UserPRs { get; set; }

    }
}
