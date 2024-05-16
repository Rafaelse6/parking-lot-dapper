using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingLotDapper.DTO;
using ParkingLotDapper.Models;
using ParkingLotDapper.Repositories;
using System.Data;

namespace ParkingLotDapper.Controllers
{
    [Route("/tickets")]
    public class TicketsController : Controller
    {
        private readonly IDbConnection _connection;
        private readonly IRepository<Ticket> _repository;

        public TicketsController(IDbConnection connection)
        {
            _connection = connection;
            _repository = new RepositoryDapper<Ticket>(_connection);
        }

        public IActionResult Index()
        {
            var sql = """
            SELECT t.*, v.*, c.*, vg.* FROM tickets t
            INNER JOIN veiculos v ON v.id = t.veiculoId
            INNER JOIN clientes c ON c.id = v.clienteId
            INNER JOIN vagas vg ON vg.id = t.vagaId
            order by t.id desc
        """;

            var tickets = _connection.Query<Ticket, Veiculo, Cliente, Vaga, Ticket>(sql, (ticket, veiculo, cliente, vaga) => {
                veiculo.Cliente = cliente;
                ticket.Veiculo = veiculo;
                ticket.Vaga = vaga;
                return ticket;
            }, splitOn: "Id, Id, Id");

            ViewBag.ValorDoMinuto = _connection.QueryFirstOrDefault<ValorDoMinuto>("select * from valores order by id desc limit 1")!;
            return View(tickets);
        }

        [HttpGet("novo")]
        public IActionResult New()
        {
            fillSpaceViewBag();
            return View();
        }

        [HttpPost("Criar")]
        public IActionResult Create([FromForm] TicketDTO ticketDTO)
        {
            Cliente cliente = searchOrCreateClientByDTO(ticketDTO);
            Veiculo veiculo = searchOrCreateVehicleByDTO(ticketDTO, cliente);

            var ticket = new Ticket();

            ticket.VeiculoId = veiculo.Id;
            ticket.DataEntrada = DateTime.Now;
            ticket.VagaId = ticketDTO.VagaId;

            _repository.Create(ticket);

            changeSpaceStatus(ticket.VagaId, true);

            return Redirect("/tickets");
        }

        [HttpPost("{id}/apagar")]
        public IActionResult Delete([FromRoute] int id)
        {
           var ticket = _repository.FindById(id);
            changeSpaceStatus(ticket.VagaId, false);
            _repository.Delete(id);
            return Redirect("/tickets");
        }

        [HttpPost("{id}/pago")]
        public IActionResult Pago([FromRoute] int id)
        {
            var sql = """
            SELECT t.*, v.*, c.*, vg.* FROM tickets t
            INNER JOIN veiculos v ON v.id = t.veiculoId
            INNER JOIN clientes c ON c.id = v.clienteId
            INNER JOIN vagas vg ON vg.id = t.vagaId
            where t.id = @id
        """;

            Ticket? ticket = _connection.Query<Ticket, Veiculo, Cliente, Vaga, Ticket>(sql, (ticket, veiculo, cliente, vaga) => {
                veiculo.Cliente = cliente;
                ticket.Veiculo = veiculo;
                ticket.Vaga = vaga;
                return ticket;
            }, new { id = id }, splitOn: "Id, Id, Id").FirstOrDefault();

            if (ticket != null)
            {
                var valorDoMinuto = _connection.QueryFirstOrDefault<ValorDoMinuto>("select * from valores order by id desc limit 1")!;

                ticket.Pago(valorDoMinuto);
               _repository.Update(ticket);
                changeSpaceStatus(ticket.VagaId, false);
            }
            return Redirect("/tickets");
        }

        private void fillSpaceViewBag()
        {
            var sql = """
            SELECT * FROM vagas 
            where Ocupada = false
        """;

            var vagas = _connection.Query<Vaga>(sql);

            ViewBag.Vagas = new SelectList(vagas, "Id", "CodigoLocalizacao");
        }

        private Cliente searchOrCreateClientByDTO(TicketDTO ticketDTO)
        {
            Cliente? cliente = null;

            if (!string.IsNullOrEmpty(ticketDTO.Cpf))
            {
                var query = "SELECT * FROM clientes where Cpf = @Cpf";
                cliente = _connection.QueryFirstOrDefault<Cliente>(query, new Cliente { Cpf = ticketDTO.Cpf });
            }

            if (cliente != null) return cliente;

            cliente = new Cliente();
            cliente.Nome = ticketDTO.Nome;
            cliente.Cpf = ticketDTO.Cpf;

            string sql = @"INSERT INTO clientes (Nome, CPF) VALUES (@Nome, @Cpf); SELECT LAST_INSERT_ID();";

            cliente.Id = _connection.QuerySingle<int>(sql, cliente);

            return cliente;
        }

        private Veiculo searchOrCreateVehicleByDTO(TicketDTO ticketDTO, Cliente cliente)
        {
            Veiculo? veiculo = null;

            if (!string.IsNullOrEmpty(ticketDTO.Placa))
            {
                var query = "SELECT * FROM veiculos where placa = @Placa and ClienteId = @ClienteId";
                veiculo = _connection.QueryFirstOrDefault<Veiculo>(query, new Veiculo { Placa = ticketDTO.Placa, ClienteId = cliente.Id });
            }

            if (veiculo != null) return veiculo;

            veiculo = new Veiculo();
            veiculo.Placa = ticketDTO.Placa;
            veiculo.Marca = ticketDTO.Marca;
            veiculo.Modelo = ticketDTO.Modelo;
            veiculo.ClienteId = cliente.Id;

            var sql = $"INSERT INTO veiculos (Placa, Marca, Modelo, ClienteId) VALUES (@Placa, @Marca, @Modelo, @ClienteId); SELECT LAST_INSERT_ID()";
            veiculo.Id = _connection.QuerySingle<int>(sql, veiculo);

            return veiculo;
        }

        private void changeSpaceStatus(int VagaId, bool ocupada)
        {
            var sql = $"UPDATE vagas SET ocupada = @Ocupada where id = @Id";
            _connection.Execute(sql, new Vaga { Id = VagaId, Ocupada = ocupada });
        }
    }
}
