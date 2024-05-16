using ParkingLotDapper.Models;

[TestClass]
public class TicketTest
{
    [TestMethod]
    public void TestingTotalValueMethod()
    {
        // Arrange 
        var cliente = new Cliente { Id = 1, Nome = "Danilo" };
        var veiculo = new Veiculo { Id = 1, Marca = "Fiat", Modelo = "Uno", ClienteId = cliente.Id, Placa = "DASS134" };
        var ticket = new Ticket { DataEntrada = DateTime.Now.AddHours(-1), Id = 1, VagaId = 1, VeiculoId = veiculo.Id };
        var valorDoMinuto = new ValorDoMinuto { Minutos = 1, Valor = 1 };
        var totalDesejadoDeMinuto = 60.0;

        // Act 
        var valorTotal = ticket.TotalValue(valorDoMinuto);

        // Assert 
        Assert.AreEqual(totalDesejadoDeMinuto, valorTotal);
    }

    public void TestingTicketPaidValue()
    {
        // Arrange 
        var cliente = new Cliente { Id = 1, Nome = "Danilo" };
        var veiculo = new Veiculo { Id = 1, Marca = "Fiat", Modelo = "Uno", ClienteId = cliente.Id, Placa = "DASS134" };
        var ticket = new Ticket { DataEntrada = DateTime.Now.AddHours(-1), Id = 1, VagaId = 1, VeiculoId = veiculo.Id };
        var valorDoMinuto = new ValorDoMinuto { Minutos = 1, Valor = 1 };
        var valorTotalDesejado = ticket.TotalValue(valorDoMinuto);

        // Act 
        ticket.Pago(valorDoMinuto);

        // Assert 
        Assert.AreEqual(valorTotalDesejado, ticket.Valor);
        Assert.IsNotNull(ticket.DataSaida);
    }
}
