using Microsoft.EntityFrameworkCore;

namespace Sistema_de_Restaurante___Modulo_de_Administracion.Models
{
	public class restauranteDbContext : DbContext

	{
		public restauranteDbContext(DbContextOptions options) : base(options)
		{
		}

		//Las tablas con sus modelos aqui :)


	}
}