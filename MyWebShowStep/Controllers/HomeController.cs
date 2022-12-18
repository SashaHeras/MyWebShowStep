using MyWebShowStep.Data;
using MyWebShowStep.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace MyWebShowStep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private StepShopContext _context = new StepShopContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AuthUser()
        {
            bool isUser = false;

            var user = _context.Users.Where(u => u.Email == Request.Form["Email"].ToString() && u.Password == Request.Form["Password"].ToString()).FirstOrDefault();

            if(user != null)
            {
                isUser = true;

                return Json(new { auth = isUser });
            }

            return Json(new { auth = isUser });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}