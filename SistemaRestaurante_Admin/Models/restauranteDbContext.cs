using Microsoft.EntityFrameworkCore;
using SistemaRestaurante_Admin.Models;
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
		public DbSet<Empleados> Empleados { get; set; }
		public DbSet<Platos> Platos { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Combos> Combos { get; set; }
        public DbSet<Platos_Combos> Platos_Combo { get; set; }


        public DbSet<cargo> cargo { get; set; }
        public DbSet<mesas> mesas { get; set; }

    }
}