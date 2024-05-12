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
            //// first or default
            // var parametro = new { Id = 1 };
            // var sql = "SELECT * FROM valores WHERE Id = @Id";
            // var resultado = _connection.QueryFirstOrDefault<ValorDoMinuto>(sql, parametro);



            //// inserir mais de um registro
            // var registrosParaInserir = new List<ValorDoMinuto>
            // {
            //     new ValorDoMinuto { Valor = 5, Minutos = 3 },
            //     new ValorDoMinuto { Valor = 15, Minutos = 20 },
            // };

            // var sql = "INSERT INTO valores (Minutos, Valor) VALUES (@Minutos, @Valor)";
            // _connection.Execute(sql, registrosParaInserir);



            //// scalar
            // var sql = "SELECT COUNT(*) FROM valores";
            // var quantidade = _connection.ExecuteScalar<int>(sql);




            //// begin trasaction
            // _connection.Open();
            // using (var transaction = _connection.BeginTransaction())
            // {
            //     try
            //     {
            //         var obj = new ValorDoMinuto{
            //             Minutos = 10,
            //             Valor = 5
            //         };

            //         // Opera��es dentro da transa��o
            //         var inserirSql = "INSERT INTO valores (Minutos, Valor) VALUES (@Minutos, @Valor);SELECT LAST_INSERT_ID();";
            //         obj.Id = _connection.ExecuteScalar<int>(inserirSql, obj, transaction);

            //         obj.Valor = 6;

            //         var atualizarSql = "UPDATE valores SET Minutos = @Minutos, valor = @Valor where id = @id";
            //         _connection.Execute(atualizarSql, obj, transaction);

            //         //throw new Exception("Parar aqui");
            //         // Commit da transa��o se tudo estiver correto
            //         transaction.Commit();
            //     }
            //     catch (Exception)
            //     {
            //         // Rollback da transa��o em caso de erro
            //         transaction.Rollback();
            //     }
            // }
            // _connection.Close();




            //// inner join
            // var sql = """
            //     SELECT 
            //         t.*, v.*, c.* 
            //     FROM tickets t
            //     JOIN veiculos v ON t.veiculoId = v.id
            //     JOIN clientes c ON v.clienteId = c.id
            // """;
            // var resultados = _connection.Query<Ticket, Veiculo, Cliente, Ticket>(sql, (ticket, veiculo, cliente) =>
            // {
            //     ticket.Veiculo = veiculo;
            //     veiculo.Cliente = cliente;
            //     return ticket;
            // }, splitOn: "id, id");



            ////pagina��o
            // int paginaAtual = 1;
            // int itensPorPagina = 10;
            // string sql = "SELECT * FROM clientes ORDER BY Nome LIMIT @PageSize OFFSET @Offset";
            // var parametros = new { Offset = (paginaAtual - 1) * itensPorPagina, PageSize = itensPorPagina };

            // var clientes = _connection.Query<Cliente>(sql, parametros);



            //// multi select em uma transa��o
            // var sql = "SELECT * FROM Clientes; SELECT * FROM tickets;";
            // using (var multi = _connection.QueryMultiple(sql))
            // {
            //     var clientes = multi.Read<Cliente>();
            //     var tickets = multi.Read<Ticket>();
            // }




            //// busca com mais de um parametro
            // var parametros = new { Nome = "%Jo�o%", Cpf = "123.456.789-01" };
            // var sql = "SELECT * FROM clientes WHERE Nome LIKE @Nome AND Cpf = @Cpf";
            // var resultado = _connection.QueryFirstOrDefault(sql, parametros);






            //// pr�xima aula procedures e views

            /*
            DELIMITER //
            CREATE PROCEDURE ObterTicketsPorCliente(IN idCliente INT)
            BEGIN
                SELECT t.*, c.nome AS NomeCliente
                FROM tickets t
                JOIN veiculos v ON t.veiculoId = v.id
                JOIN clientes c ON v.clienteId = c.id
                WHERE c.id = idCliente;
            END //
            DELIMITER ;
            CREATE VIEW VistaTicketsComClientes AS
            SELECT t.*, c.nome AS NomeCliente
            FROM tickets t
            JOIN veiculos v ON t.veiculoId = v.id
            JOIN clientes c ON v.clienteId = c.id;
            CALL ObterTicketsPorCliente(ID_DESEJADO);

            SELECT * FROM VistaTicketsComClientes;
            public class TicketComCliente
            {
                public Ticket Ticket { get; set; }
                public string NomeCliente { get; set; }
                // Outras propriedades de cliente conforme necess�rio
            }
            */
            // para executar uma procedure

            // var parameters = new DynamicParameters();
            // parameters.Add("@nome", "Danilo");
            // var customers = _connection.Query<Cliente>("sp_GetClientes", parameters, commandType: CommandType.StoredProcedure);

            // var parametros = new DynamicParameters();
            // parametros.Add("idCliente", 1);
            // var ticketsClientes = _connection.Query<Ticket, string, TicketComCliente>(
            //     "ObterTicketsPorCliente", 
            //     (ticket, nomeCliente) => 
            //     {
            //         return new TicketComCliente { Ticket = ticket, NomeCliente = nomeCliente };
            //     }, 
            //     parametros, splitOn: "NomeCliente",
            //     commandType: CommandType.StoredProcedure
            // );

            //// executar views
            // var sqlView = "SELECT * FROM VistaTicketsComClientes";
            // var ticketsComClientes = _connection.Query<TicketComCliente>(sqlView);


            var values = _connection.Query<ValorDoMinuto>("SELECT * FROM valores").ToList();

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
            var sql = "INSERT INTO valores (Minutos, Valor) VALUES (@Minutos, @Valor)";
            _connection.Execute(sql, valorDoMinuto);

            return Redirect("/valores");
        }

        [HttpPost("{id}/apagar")]
        public IActionResult Delete([FromRoute] int id)
        {
            var sql = "DELETE FROM valores WHERE id=@id";
            _connection.Execute(sql, new ValorDoMinuto { Id = id });

            return Redirect("/valores");
        }

        [HttpPost("{id}/alterar")]
        public IActionResult Update([FromRoute] int id, [FromForm] ValorDoMinuto valorDoMinuto)
        {
            valorDoMinuto.Id = id;

            var sql = "UPDATE valores SET Minutos = @Minutos, valor = @Valor where id = @id";
            _connection.Execute(sql, valorDoMinuto);

            return Redirect("/valores");
        }

        [HttpGet("{id}/editar")]
        public IActionResult Edit([FromRoute] int id)
        {
            var valor = _connection.Query<ValorDoMinuto>("SELECT * FROM valores where id = @id", new ValorDoMinuto { Id = id }).FirstOrDefault();
            return View(valor);
        }
    }
}
