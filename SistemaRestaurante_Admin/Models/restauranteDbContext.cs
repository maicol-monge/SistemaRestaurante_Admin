using Microsoft.EntityFrameworkCore;
using SistemaRestaurante_Admin.Models;
using SistemaRestaurante_Admin.Models.Cargos;
using SistemaRestaurante_Admin.Models.Mesas;
using static Azure.Core.HttpHeader;

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

		public DbSet<cargo> cargo { get; set; }
        public DbSet<mesas> mesas { get; set; }

        public DbSet<Promocion> Promociones { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboPromocion> ComboPromocion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ComboPromocion>().ToTable("combo_promocion");

            
            modelBuilder.Entity<ComboPromocion>().HasKey(cp => cp.id);

           
            modelBuilder.Entity<ComboPromocion>()
                .HasOne(cp => cp.Combo)
                .WithMany()
                .HasForeignKey(cp => cp.combo_id)
                .HasConstraintName("FK_combo_promocion_combos");

            modelBuilder.Entity<ComboPromocion>()
                .HasOne(cp => cp.Promocion)
                .WithMany(p => p.ComboPromociones)
                .HasForeignKey(cp => cp.promocion_id)
                .HasConstraintName("FK_combo_promocion_promociones");

            modelBuilder.Entity<Promocion>(entity =>
            {
                entity.ToTable("promociones");
                entity.HasKey(p => p.id);
                entity.Property(p => p.id).HasColumnName("id");
                entity.Property(p => p.nombre).HasColumnName("nombre");
                entity.Property(p => p.descripcion).HasColumnName("descripcion");
                entity.Property(p => p.descuento).HasColumnName("descuento");
                entity.Property(p => p.estado).HasColumnName("estado");
                entity.Property(p => p.categoria_id).HasColumnName("categoria_id");

                entity.HasOne(p => p.Categoria)
                      .WithMany()
                      .HasForeignKey(p => p.categoria_id);
            });

           
            modelBuilder.Entity<ComboPromocion>()
                .Property(cp => cp.id)
                .HasColumnName("id");

            modelBuilder.Entity<ComboPromocion>()
                .Property(cp => cp.combo_id)
                .HasColumnName("combo_id");

            modelBuilder.Entity<ComboPromocion>()
                .Property(cp => cp.promocion_id)
                .HasColumnName("promocion_id");

            modelBuilder.Entity<ComboPromocion>()
                .Property(cp => cp.fecha_inicio)
                .HasColumnName("fecha_inicio");

            modelBuilder.Entity<ComboPromocion>()
                .Property(cp => cp.fecha_fin)
                .HasColumnName("fecha_fin");

            modelBuilder.Entity<ComboPromocion>()
                .Property(cp => cp.estado)
                .HasColumnName("estado");
        }
    }
}