using Microsoft.AspNetCore.Mvc;
using ParkingLotDapper.Models;
using ParkingLotDapper.Repositories;

namespace ParkingLotDapper.Controllers
{
    [Route("/valores")]
    public class ValorDoMinutoController : Controller
    {
        private readonly IRepository<ValorDoMinuto> _repository;
        private readonly IRepository<Veiculo> _repoVeic;

        public ValorDoMinutoController(IRepository<ValorDoMinuto> repository, IRepository<Veiculo> repoVeic)
        {
            _repository = repository;
            _repoVeic = repoVeic;
        }

        public IActionResult Index()
        {
            var vehicles = _repoVeic.FindAll();
            var values = _repository.FindAll();
            return View(values);
        }

        [HttpGet("novo")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("Criar")]
        public IActionResult Create([FromForm] ValorDoMinuto valorDoMinuto)
        {
            _repository.Create(valorDoMinuto);

            return Redirect("/valores");
        }

        [HttpPost("{id}/apagar")]
        public IActionResult Delete([FromRoute] int id)
        {
            _repository.Delete(id);

            return Redirect("/valores");
        }

        [HttpPost("{id}/alterar")]
        public IActionResult Update([FromRoute] int id, [FromForm] ValorDoMinuto valorDoMinuto)
        {
            valorDoMinuto.Id = id;

            _repository.Update(valorDoMinuto);

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
