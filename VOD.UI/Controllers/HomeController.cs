using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VOD.Common.Entities;
using VOD.UI.Models;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<VodUser> _singInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<VodUser> singInMgr)
        {
            _singInManager = singInMgr;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!_singInManager.IsSignedIn(User))
                return RedirectToPage(
                    "/Account/Login", new {Area = "Identity"});

            return RedirectToAction("Dashboard", "Membership");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}