namespace SistemaRestaurante_Admin.Models
{
    public class Empleados
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public string contrasena { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public int cargo_Id { get; set; }
        public int estado { get; set; }
    }
}
