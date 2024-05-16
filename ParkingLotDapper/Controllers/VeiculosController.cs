using Dapper;
using Microsoft.AspNetCore.Mvc;
using ParkingLotDapper.Models;
using ParkingLotDapper.Repositories;
using System.Data;

namespace ParkingLotDapper.Controllers
{
    [Route("/veiculos")]
    public class VeiculosController : Controller
    {

        private readonly IDbConnection _connection;
        private readonly IRepository<Veiculo> _repository;

        public VeiculosController(IDbConnection connection)
        {
            _connection = connection;
            _repository = new RepositoryDapper<Veiculo>(connection);
        }

        public IActionResult Index()
        {
            var sql = """
                SELECT v.*, c.* FROM veiculos v
                INNER JOIN clientes c ON c.id = v.clienteId
            """;

            var vehicles = _connection.Query<Veiculo, Cliente, Veiculo>(sql, (veiculo, cliente) =>
            {
                veiculo.Cliente = cliente;
                return veiculo;
            }, splitOn: "Id");

            return View(vehicles);
        }

        [HttpGet("novo")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("Criar")]
        public IActionResult Create([FromForm] Veiculo veiculo)
        {
            _repository.Create(veiculo);

            return Redirect("/veiculos");
        }

        [HttpPost("{id}/apagar")]
        public IActionResult Delete([FromRoute] int id)
        {
            _repository.Delete(id);

            return Redirect("/veiculos");
        }

        [HttpPost("{id}/alterar")]
        public IActionResult Update([FromRoute] int id, [FromForm] Veiculo veiculo)
        {
            veiculo.Id = id;

            _repository.Update(veiculo);

            return Redirect("/veiculos");
        }

        [HttpGet("{id}/editar")]
        public IActionResult Edit([FromRoute] int id)
        {
            var value = _repository.FindById(id);
            return View(value);
        }
    }
}
