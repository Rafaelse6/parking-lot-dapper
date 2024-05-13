using ParkingLotDapper.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingLotDapper.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [IgnoreInDapper]
        public int Id { get; set; } = default!;
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
    }
}
