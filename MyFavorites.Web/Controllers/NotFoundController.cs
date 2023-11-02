using Microsoft.AspNetCore.Mvc;

namespace MyFavorites.Web.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
