using Microsoft.AspNetCore.Mvc;

namespace MvcWhatsUp.Controllers
{
    public class ChatsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
