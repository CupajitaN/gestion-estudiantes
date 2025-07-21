using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<EstudianteMateria> EstudianteMaterias { get; set; }
        public DbSet<ProfesorMateria> ProfesorMaterias { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Estudiante - EstudianteMateria - Materia
            modelBuilder.Entity<EstudianteMateria>()
                .HasOne(em => em.Estudiante)
                .WithMany(e => e.EstudianteMaterias)
                .HasForeignKey(em => em.EstudianteId);

            modelBuilder.Entity<EstudianteMateria>()
                .HasOne(em => em.Materia)
                .WithMany(m => m.EstudianteMaterias)
                .HasForeignKey(em => em.MateriaId);

            // Relación Profesor - ProfesorMateria - Materia
            modelBuilder.Entity<ProfesorMateria>()
                .HasOne(pm => pm.Profesor)
                .WithMany(p => p.ProfesorMaterias)
                .HasForeignKey(pm => pm.ProfesorId);

            modelBuilder.Entity<ProfesorMateria>()
                .HasOne(pm => pm.Materia)
                .WithMany(m => m.ProfesorMaterias)
                .HasForeignKey(pm => pm.MateriaId);

            // Valores por defecto
            modelBuilder.Entity<Materia>()
                .Property(m => m.Creditos)
                .HasDefaultValue(3);

            modelBuilder.Entity<Estudiante>()
                .Property(e => e.CreditosMaximos)
                .HasDefaultValue(9);

            modelBuilder.Entity<Profesor>()
                .Property(p => p.MateriasMax)
                .HasDefaultValue(2);

            modelBuilder.Entity<Estudiante>()
                .HasOne(e => e.Usuario)
                .WithOne(u => u.Estudiante)
                .HasForeignKey<Estudiante>(e => e.UsuarioId)
                .IsRequired(false);
        }
    }
}
