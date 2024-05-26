namespace MovieRental.Models
{
    public class RegisterViewModel
    {
        public required int Uid { get; set; }
        public required string UserName { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
