using Microsoft.AspNetCore.Mvc;

namespace MoneySaverApi.Controllers
{
    public class UserKycController : Controller
    {
        public IActionResult Index()
        {
            return View();     
        }
    }
}
