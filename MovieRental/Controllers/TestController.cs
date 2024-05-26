using Microsoft.AspNetCore.Mvc;

namespace MovieRental.Controllers
{
	public class TestController : Controller
	{
		public IActionResult SetSession()
		{
			HttpContext.Session.SetString("TestKey", "TestValue");
			return Content("Session value set.");
		}
		public IActionResult GetSession()
		{
			var value = HttpContext.Session.GetString("TestKey");
			return Content($"Session value: {value}");
		}
	}
}
