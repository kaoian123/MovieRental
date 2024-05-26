using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.Entities;
using MovieRental.Services;

namespace MovieRental.Controllers
{
    public class MemberController(MovieRentalDBContext context, MemberService memberService, EmailSender emailSender) : Controller
    {
		private readonly MovieRentalDBContext _context = context;
        private readonly MemberService _memberService = memberService;
        private readonly EmailSender _emailSender = emailSender;
        [HttpGet]
        public IActionResult Login()
		{
			return View();
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Member? member = await _memberService.SearchMember(model.Email);
            if (member != null)
            {
                var result = await _memberService.PasswordSignInAsync(model.Email, model.Password);
                if (result.Succeeded)
                {
                    HttpContext.Session.SetInt32("Uid", member.Uid);

					return RedirectToAction("Index", "Movie");
                }
            }
            TempData["ErrorMessage"] = "帳號或密碼錯誤";
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile Avatar)
        {
            Member? member = await _memberService.SearchMember(model.Email);
            if (member != null)
            {
                TempData["ErrorMessage"] = "信箱已被註冊";
                return View(model);
            }
            if (Avatar != null)
            {
                using var ms = new MemoryStream();
                Avatar.CopyTo(ms);
                model.Avatar = ms.ToArray();
            }
            DateTime dateTime = DateTime.UtcNow;
            member = new Member
            {
                UserName = model.UserName,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Avatar = model.Avatar,
                AccountStatus = 1,
                Created_at = dateTime.ToString()
            };
            var result = await _memberService.CreateAsync(member, model.Password);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "註冊成功";
                return RedirectToAction(nameof(Login));
            }
            TempData["ErrorMessage"] = "發生未知錯誤，請聯絡管理員";
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            Member? member = await _memberService.SearchMember(model.Email);
            if (member != null)
            {
                var token = await _memberService.GeneratePasswordResetTokenAsync(member);
                Console.WriteLine(token);
                var callbackUrl = Url.Action
                    (
                        nameof(ResetPassword),
                        "Member",
                        new { token, email = model.Email },
                        Request.Scheme
                    );
                await _emailSender.SendPasswordResetEmailAsync(model.Email, callbackUrl);
            }
            TempData["SuccessMessage"] = "驗證信已寄出";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string? token = null, string? email = null)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email) || !await _memberService.ValidatePasswordResetToken(token, email, 0))
            {
                return NotFound();
            }
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.Email) || !await _memberService.ValidatePasswordResetToken(model.Token, model.Email, 1))
            {
                return NotFound();
            }
            Member? member = await _memberService.SearchMember(model.Email);
            if (member != null)
            {
                var result = await _memberService.ResetPasswordAsync(member, model.Password);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "密碼已成功重設";
                    return RedirectToAction("Login"); ;
                }
            }
            TempData["ErrorMessage"] = "發生未知錯誤，請聯絡管理員";
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}