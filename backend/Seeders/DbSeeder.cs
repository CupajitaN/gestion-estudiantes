using Backend.Models;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Backend.Data
{
    public static class DbSeeder
    {
        public static void SeedUsuarios(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            var usuarioService = serviceProvider.GetRequiredService<IUsuarioService>();

            if (!context.Usuarios.Any(u => u.Correo == "admin@correo.com"))
            {
                var usuario = new Usuario
                {
                    Correo = "admin@correo.com",
                    ContrasenaHash = usuarioService.HashearContrasena("admin"),
                    Rol = "admin"
                };

                context.Usuarios.Add(usuario);
                context.SaveChanges();
            }
        }

        public static void SeedMaterias(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (!context.Materias.Any())
            {
                var materias = new List<Materia>
                {
                    new() { Nombre = "Matemáticas I", Descripcion = "Álgebra, aritmética y lógica básica" },
                    new() { Nombre = "Física General", Descripcion = "Conceptos fundamentales de la física clásica" },
                    new() { Nombre = "Programación I", Descripcion = "Fundamentos de programación estructurada" },
                    new() { Nombre = "Programación II", Descripcion = "Estructuras de datos y POO" },
                    new() { Nombre = "Bases de Datos", Descripcion = "Modelado relacional, SQL y transacciones" },
                    new() { Nombre = "Estadística", Descripcion = "Probabilidad, distribuciones y análisis estadístico" },
                    new() { Nombre = "Inglés Técnico", Descripcion = "Lectura y comprensión de textos técnicos en inglés" },
                    new() { Nombre = "Redes I", Descripcion = "Fundamentos de redes de computadoras y protocolos" },
                    new() { Nombre = "Ingeniería de Software", Descripcion = "Ciclo de vida del software y metodologías ágiles" },
                    new() { Nombre = "Lógica de Programación", Descripcion = "Pensamiento lógico aplicado a algoritmos" }
                };

                context.Materias.AddRange(materias);
                context.SaveChanges();
            }
        }

        public static void SeedProfesores(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Profesores.Any()) return;

            var profesores = new List<Profesor>
            {
                new() { Nombre = "Ana", Apellidos = "Pérez", Correo = "ana@uni.edu", Jornada = "Mañana" },
                new() { Nombre = "Luis", Apellidos = "Martínez", Correo = "luis@uni.edu", Jornada = "Tarde" },
                new() { Nombre = "Carlos", Apellidos = "López", Correo = "carlos@uni.edu", Jornada = "Noche" },
                new() { Nombre = "Diana", Apellidos = "Torres", Correo = "diana@uni.edu", Jornada = "Mañana" },
                new() { Nombre = "Mateo", Apellidos = "Gómez", Correo = "mateo@uni.edu", Jornada = "Tarde" }
            };

            context.Profesores.AddRange(profesores);
            context.SaveChanges();
        }

        public static void SeedEstudiantesYUsuarios(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            var usuarioService = serviceProvider.GetRequiredService<IUsuarioService>();

            if (!context.Estudiantes.Any())
            {
                // 1. Crear estudiantes
                var estudiante1 = new Estudiante
                {
                    Nombre = "Camila",
                    Apellidos = "Ríos",
                    Correo = "camila@uni.edu",
                    Programa = "Ingeniería de Sistemas",
                    Jornada = "Mañana",
                    CreditosMaximos = 9,
                    CreditosUtilizados = 0
                };

                var estudiante2 = new Estudiante
                {
                    Nombre = "Juan",
                    Apellidos = "Mejía",
                    Correo = "juan@uni.edu",
                    Programa = "Administración",
                    Jornada = "Tarde",
                    CreditosMaximos = 9,
                    CreditosUtilizados = 0
                };

                context.Estudiantes.AddRange(estudiante1, estudiante2);
                context.SaveChanges();

                // 2. Crear usuarios para esos estudiantes
                var usuario1 = new Usuario
                {
                    Correo = estudiante1.Correo,
                    ContrasenaHash = usuarioService.HashearContrasena("camila123"),
                    Rol = "estudiante",
                    EstudianteId = estudiante1.Id
                };

                var usuario2 = new Usuario
                {
                    Correo = estudiante2.Correo,
                    ContrasenaHash = usuarioService.HashearContrasena("juan123"),
                    Rol = "estudiante",
                    EstudianteId = estudiante2.Id
                };

                context.Usuarios.AddRange(usuario1, usuario2);
                context.SaveChanges();

                // 3. Ahora actualizas el UsuarioId en los estudiantes
                estudiante1.UsuarioId = usuario1.Id;
                estudiante2.UsuarioId = usuario2.Id;

                context.SaveChanges(); // Guardar la relación inversa
            }

            // Profesores
            if (!context.Usuarios.Any(u => u.Rol == "profesor"))
            {
                var profesor1 = context.Profesores.FirstOrDefault(p => p.Correo == "ana@uni.edu");
                var profesor2 = context.Profesores.FirstOrDefault(p => p.Correo == "luis@uni.edu");

                if (profesor1 != null && profesor2 != null)
                {
                    var usuariosProfesores = new List<Usuario>
                    {
                        new Usuario
                        {
                            Correo = profesor1.Correo,
                            ContrasenaHash = usuarioService.HashearContrasena("ana123"),
                            Rol = "profesor",
                            ProfesorId = profesor1.Id
                        },
                        new Usuario
                        {
                            Correo = profesor2.Correo,
                            ContrasenaHash = usuarioService.HashearContrasena("luis123"),
                            Rol = "profesor",
                            ProfesorId = profesor2.Id
                        }
                    };

                    context.Usuarios.AddRange(usuariosProfesores);
                    context.SaveChanges();
                }
            }
        }

    }
}
