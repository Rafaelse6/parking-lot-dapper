namespace ParkingLotDapper.Models
{
    public class Cliente
    {
        public int Id { get; set; } = default!;
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
    }
}
