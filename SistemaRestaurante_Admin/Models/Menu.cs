namespace SistemaRestaurante_Admin.Models
{
    public class Menu
    {
        //tipo_menu, tipo_venta, hora_inicio, hora_fin, fecha_inicio, fecha_fin, estado
        public int id { get; set; }
        public string? tipo_menu { get; set; }
        public string? tipo_venta { get; set; }
        public TimeSpan? hora_inicio { get; set; }
        public TimeSpan? hora_fin { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_fin { get; set; }
        public int estado { get; set; }


        public class CrearMenuRequest
        {
            public int id { get; set; }
            public List<int>? PlatosSeleccionados { get; set; }
            public List<int>? CombosSeleccionados { get; set; }
            public string? TipoMenu { get; set; }
            public string? TipoVenta { get; set; }
            public TimeSpan? HoraInicio { get; set; }
            public TimeSpan? HoraFin { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
        }



    }
}
