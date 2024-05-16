using ParkingLotDapper.Models;

namespace Tests
{

    [TestClass]
    public class ClienteTest
    {
        [TestMethod]
        public void TestingClienteModel()
        {
            // Arrange
            var cliente = new Cliente();

            //  Act
            cliente.Id = 1;
            cliente.Nome = "Danilo";
            cliente.Cpf = "653.027.290-91";

            // Assert
            Assert.AreEqual(1, cliente.Id);
            Assert.AreEqual("Danilo", cliente.Nome);
            Assert.AreEqual("653.027.290-91", cliente.Cpf);
        }
    }
}
