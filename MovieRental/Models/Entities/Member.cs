using Microsoft.EntityFrameworkCore;

namespace MovieRental.Models.Entities
{
    [PrimaryKey(nameof(Uid))]
    public class Member
    {
        public int Uid { get; set; }
        public required string UserName { get; set; }
        public required string Name { get; set;}
        public required string Email { get; set; }  
        public required string Password { get; set; }
        public byte[]? Avatar { get; set;}
        public int AccountStatus { get; set;}
        public string? Created_at { get; set;}
        public string? PasswordResetToken { get; set; }
    }
}
