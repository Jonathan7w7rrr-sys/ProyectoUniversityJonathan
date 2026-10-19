using Microsoft.EntityFrameworkCore;
using ProyectoUniversityJonathan.Models;

namespace ProyectoUniversityJonathan.Data;

public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
{
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Docente> Docentes => Set<Docente>();
    public DbSet<Asignacion> Asignaciones => Set<Asignacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Curso>().Property(x => x.Nombre).HasMaxLength(150);
        modelBuilder.Entity<Estudiante>().Property(x => x.Nombres).HasMaxLength(100);
        modelBuilder.Entity<Estudiante>().Property(x => x.Apellidos).HasMaxLength(100);
        modelBuilder.Entity<Estudiante>().Property(x => x.Direccion).HasMaxLength(250);
        modelBuilder.Entity<Docente>().Property(x => x.Nombres).HasMaxLength(100);
        modelBuilder.Entity<Docente>().Property(x => x.Apellidos).HasMaxLength(100);
        modelBuilder.Entity<Asignacion>().HasOne(x => x.Curso).WithMany(x => x.Asignaciones)
            .HasForeignKey(x => x.CursoId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Asignacion>().HasOne(x => x.Estudiante).WithMany(x => x.Asignaciones)
            .HasForeignKey(x => x.EstudianteId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Asignacion>().HasOne(x => x.Docente).WithMany(x => x.Asignaciones)
            .HasForeignKey(x => x.DocenteId).OnDelete(DeleteBehavior.Restrict);
    }
}
