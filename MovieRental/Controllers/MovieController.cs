using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Models;
using MovieRental.Models.Entities;

namespace MovieRental.Controllers
{
    public class MovieController(MovieRentalDBContext context) : Controller
    {
        private readonly MovieRentalDBContext _context = context;
		public IActionResult Index()
		{
			int? uid = HttpContext.Session.GetInt32("Uid");
			ViewData["id"] = uid;
			return View();
		}
	}
}
