﻿using System.ComponentModel.DataAnnotations;

namespace SistemaRestaurante_Admin.Models.Mesas
{
    public class mesas
    {
        [Key]
        public int id { get; set; }
        public int numero { get; set; }
        public int capacidad { get; set; }
        public string disponibilidad { get; set; }
        public int estado { get; set; }
    }
}
