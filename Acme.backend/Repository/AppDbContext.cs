using Microsoft.EntityFrameworkCore;
using Model.DataBase;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para las entidades
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Proyecto> Proyectos { get; set; } = null!;
        public DbSet<ProyectoUsuario> ProyectoUsuarios { get; set; } = null!;
        public DbSet<InvitacionProyecto> InvitacionesProyecto { get; set; } = null!;
        public DbSet<Tarea> Tareas { get; set; } = null!;
        public DbSet<TareaUsuario> TareaUsuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de índices únicos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.email)
                .IsUnique();

            // Configuración de relaciones para evitar ciclos de eliminación en cascada
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.ProyectosCreados)
                .WithOne(p => p.Propietario)
                .HasForeignKey(p => p.propietario_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proyecto>()
                .HasMany(p => p.Miembros)
                .WithOne(pu => pu.Proyecto)
                .HasForeignKey(pu => pu.proyecto_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Proyecto>()
                .HasMany(p => p.Invitaciones)
                .WithOne(i => i.Proyecto)
                .HasForeignKey(i => i.proyecto_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvitacionProyecto>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Invitaciones)
                .HasForeignKey(i => i.usuario_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.ProyectosParticipando)
                .WithOne(pu => pu.Usuario)
                .HasForeignKey(pu => pu.usuario_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TareaUsuario>()
                .HasOne(tu => tu.Usuario)
                .WithMany(u => u.TareasAsignadas)
                .HasForeignKey(tu => tu.usuario_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proyecto>()
                .HasMany(p => p.Tareas)
                .WithOne(t => t.Proyecto)
                .HasForeignKey(t => t.proyecto_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tarea>()
                .HasMany(t => t.UsuariosAsignados)
                .WithOne(tu => tu.Tarea)
                .HasForeignKey(tu => tu.tarea_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de relaciones de auditoría (evitar ciclos)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.UsuarioCreador)
                .WithMany()
                .HasForeignKey(u => u.creado_por)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.UsuarioModificador)
                .WithMany()
                .HasForeignKey(u => u.modificado_por)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.UsuarioCreador)
                .WithMany()
                .HasForeignKey(p => p.creado_por)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.UsuarioModificador)
                .WithMany()
                .HasForeignKey(p => p.modificado_por)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.UsuarioCreador)
                .WithMany()
                .HasForeignKey(t => t.creado_por)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.UsuarioModificador)
                .WithMany()
                .HasForeignKey(t => t.modificado_por)
                .OnDelete(DeleteBehavior.Restrict);

            // Query Filters para eliminación lógica
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => u.estado == 1);
            modelBuilder.Entity<Proyecto>().HasQueryFilter(p => p.estado == 1);
            modelBuilder.Entity<ProyectoUsuario>().HasQueryFilter(pu => pu.estado == 1);
            modelBuilder.Entity<Tarea>().HasQueryFilter(t => t.estado == 1);
        }
    }
}
