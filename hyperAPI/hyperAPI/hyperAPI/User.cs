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
    }
}
