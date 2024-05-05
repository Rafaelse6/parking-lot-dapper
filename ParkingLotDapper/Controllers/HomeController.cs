using Microsoft.AspNetCore.Mvc;

namespace ParkingLotDapper.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
    }
}
