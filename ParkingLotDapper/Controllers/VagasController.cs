using Microsoft.AspNetCore.Mvc;
using ParkingLotDapper.Models;
using ParkingLotDapper.Repositories;

namespace ParkingLotDapper.Controllers
{
    [Route("/vagas")]
    public class VagasController : Controller
    {
        private readonly IRepository<Vaga> _repository;

        public VagasController(IRepository<Vaga> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var values = _repository.FindAll();
            return View(values);
        }

        [HttpGet("novo")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("Criar")]
        public IActionResult Create([FromForm] Vaga vaga)
        {
            _repository.Create(vaga);

            return Redirect("/vagas");
        }

        [HttpPost("{id}/apagar")]
        public IActionResult Delete([FromRoute] int id)
        {
            _repository.Delete(id);

            return Redirect("/vagas");
        }

        [HttpPost("{id}/alterar")]
        public IActionResult Update([FromRoute] int id, [FromForm] Vaga vaga)
        {
            vaga.Id = id;

            _repository.Update(vaga);

            return Redirect("/vagas");
        }

        [HttpGet("{id}/editar")]
        public IActionResult Edit([FromRoute] int id)
        {
            var value = _repository.FindById(id);
            return View(value);
        }
    }
}
