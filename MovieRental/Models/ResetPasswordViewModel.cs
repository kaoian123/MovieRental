namespace MovieRental.Models
{
    public class ResetPasswordViewModel
    {
        public required string Token { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }

    }
}
