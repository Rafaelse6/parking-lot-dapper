using Dapper;
using Microsoft.AspNetCore.Mvc;
using ParkingLotDapper.Models;
using System.Data;

namespace ParkingLotDapper.Controllers
{
    [Route("/valores")]
    public class ValorDoMinutoController : Controller
    {
        private readonly IDbConnection _connection;

        public ValorDoMinutoController(IDbConnection connection)
        {
            _connection = connection;
        }

        public IActionResult Index()
        {

            var values = _connection.Query<ValorDoMinuto>("SELECT * FROM valores").ToList();

            return View(values);

        }
    }
}
