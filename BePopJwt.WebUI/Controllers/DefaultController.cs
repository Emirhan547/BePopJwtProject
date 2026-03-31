using Microsoft.AspNetCore.Mvc;

namespace BePopJwt.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
