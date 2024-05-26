using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Models.Entities;

namespace MovieRental.Services
{
    public class MemberService(MovieRentalDBContext context, TokenService tokenService)
    {
        private readonly MovieRentalDBContext _context = context;
        private readonly TokenService _tokenService = tokenService;

        public async Task<string> GeneratePasswordResetTokenAsync(Member member)
        {
            var token = _tokenService.GenerateToken(member.Email);
            Console.WriteLine(token);
            member.PasswordResetToken = token;
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return token;
        }
        public async Task<bool> ValidatePasswordResetToken(string token, string email, int checktimes)
        {
            var member = await _context.Members.SingleOrDefaultAsync(m => m.Email == email && m.PasswordResetToken == token);
            Console.WriteLine(member == null);
            if (member == null || !_tokenService.ValidateToken(token, email, TimeSpan.FromHours(1)))
            {
                Console.WriteLine("這4");
                return false;
            }
            if (checktimes == 1)
            {
                member.PasswordResetToken = null;
                _context.Members.Update(member);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task<Member> SearchMember(string? Email = null, int? UID = null)
        {
            Member? member = null;
            if (!string.IsNullOrEmpty(Email))
            {
                member = await _context.Members.SingleOrDefaultAsync(m => m.Email == Email); ;
            }
            if (UID.HasValue)
            {
                member = await _context.Members.SingleOrDefaultAsync(m => m.Uid == UID);
            }

            return member;
        }

        public async Task<IdentityResult> CreateAsync(Member member, string password)
        {
            var passwordHasher = new PasswordHasher<Member>();
            member.Password = passwordHasher.HashPassword(member, password);

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ResetPasswordAsync(Member member, string password)
        {
            var passwordHasher = new PasswordHasher<Member>();
            member.Password = passwordHasher.HashPassword(member, password);

            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password)
        {
            var member = await SearchMember(email);
            if (member == null)
            {
                return SignInResult.Failed;
            }

            var passwordHasher = new PasswordHasher<Member>();
            var result = passwordHasher.VerifyHashedPassword(member, member.Password, password);
            if (result == PasswordVerificationResult.Success)
            {
                // 實現簽入邏輯，例如使用 Cookies
                return SignInResult.Success;
            }
            return SignInResult.Failed;
        }

        public async Task SignOutAsync()
        {
            // 實現登出邏輯，例如使用 Cookies
        }
    }
}