namespace SistemaRestaurante_Admin.Models
{
    public class Platos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public int Categoria_Id { get; set; }
        public int Estado { get; set; }
    }
}
