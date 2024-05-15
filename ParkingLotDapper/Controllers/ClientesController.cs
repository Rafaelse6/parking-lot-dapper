using Microsoft.AspNetCore.Mvc;
using ParkingLotDapper.Models;
using ParkingLotDapper.Repositories;

namespace ParkingLotDapper.Controllers
{
    [Route("/clientes")]
    public class ClienteController : Controller
    {
        private readonly IRepository<Cliente> _repository;


        public ClienteController(IRepository<Cliente> repository)
        {
            _repository = repository;

        }

        public IActionResult Index()
        {
            var clientes = _repository.FindAll();
            return View(clientes);
        }

        [HttpGet("novo")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("Criar")]
        public IActionResult Create([FromForm] Cliente client)
        {
            _repository.Create(client);

            return Redirect("/clientes");
        }

        [HttpPost("{id}/apagar")]
        public IActionResult Delete([FromRoute] int id)
        {
            _repository.Delete(id);

            return Redirect("/clientes");
        }

        [HttpPost("{id}/alterar")]
        public IActionResult Update([FromRoute] int id, [FromForm] Cliente cliente)
        {
            cliente.Id = id;

            _repository.Update(cliente);

            return Redirect("/valores");
        }

        [HttpGet("{id}/editar")]
        public IActionResult Edit([FromRoute] int id)
        {
            var value = _repository.FindById(id);
            return View(value);
        }
    }
}
