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

        [HttpGet("/novo")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("/Criar")]
        public IActionResult Create([FromForm] ValorDoMinuto valorDoMinuto)
        {
            var sql = "INSERT INTO valores (Minutos, Valor) VALUES (@Minutos, @Valor)";
            _connection.Execute(sql, valorDoMinuto);

            return Redirect("/valores");
        }
    }
}
