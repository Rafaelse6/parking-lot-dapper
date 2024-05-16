using ParkingLotDapper.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingLotDapper.Models
{
    [Table("tickets")]
    public class Ticket
    {
        [IgnoreInDapper]
        public int Id { get; set; } = default!;
        public DateTime DataEntrada { get; set; } = default!;
        public DateTime? DataSaida { get; set; } // Nullable para permitir valores nulos
        public float Valor { get; set; } = default!;
        public int VeiculoId { get; set; } = default!;

        [IgnoreInDapper]
        public Veiculo Veiculo { get; set; } = default!;
        public int VagaId { get; set; } = default!;
        [IgnoreInDapper]
        public Vaga Vaga { get; set; } = default!;

        public float TotalValue(ValorDoMinuto valorDoMinuto)
        {
            if (this.DataSaida != null) return this.Valor;

            var valorMinuto = valorDoMinuto.Valor / valorDoMinuto.Minutos;

            TimeSpan difference = DateTime.Now - this.DataEntrada;
            int minutos = (int)difference.TotalMinutes;

            return minutos * valorMinuto;
        }

        public void Pago(ValorDoMinuto valorDoMinuto)
        {
            this.Valor = this.TotalValue(valorDoMinuto);
            this.DataSaida = DateTime.Now;
        }
    }
}