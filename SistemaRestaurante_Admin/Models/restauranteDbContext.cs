using Microsoft.EntityFrameworkCore;
using SistemaRestaurante_Admin.Models.Cargos;
using SistemaRestaurante_Admin.Models.Mesas;

namespace Sistema_de_Restaurante___Modulo_de_Administracion.Models
{
	public class restauranteDbContext : DbContext

	{
		public restauranteDbContext(DbContextOptions options) : base(options)
		{
		}

        //Las tablas con sus modelos aqui :)

        public DbSet<cargo> cargo { get; set; }
		public DbSet<mesas> mesas { get; set; }

    }
}