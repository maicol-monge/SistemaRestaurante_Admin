using Microsoft.EntityFrameworkCore;
using SistemaRestaurante_Admin.Models;

namespace Sistema_de_Restaurante___Modulo_de_Administracion.Models
{
	public class restauranteDbContext : DbContext

	{
		public restauranteDbContext(DbContextOptions options) : base(options)
		{
		}

		//Las tablas con sus modelos aqui :)
		public DbSet<Empleados> Empleados { get; set; }
		public DbSet<Platos> Platos { get; set; }
		public DbSet<Categoria> Categoria { get; set; }


    }
}