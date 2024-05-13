﻿using ParkingLotDapper.Repositories;

namespace ParkingLotDapper.Models
{
    public class Veiculo
    {
        [IgnoreInDapper]
        public int Id { get; set; } = default!;
        public string Placa { get; set; } = default!;
        public string Modelo { get; set; } = default!;
        public string Marca { get; set; } = default!;
        public int ClienteId { get; set; } = default!;

        [IgnoreInDapper]
        public Cliente Cliente { get; set; } = default!;
    }
}
