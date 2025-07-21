using Backend.Interfaces;
using Backend.Models;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .Include(u => u.Estudiante)
                .Include(u => u.Profesor)
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task<Usuario?> CrearUsuarioAsync(string correo, string contrasena, string rol, int? estudianteId, int? profesorId)
        {
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuarioExistente != null)
            {
                return null;
            }

            var hash = HashearContrasena(contrasena);

            var usuario = new Usuario
            {
                Correo = correo,
                ContrasenaHash = hash,
                Rol = rol,
                EstudianteId = estudianteId,
                ProfesorId = profesorId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public bool VerificarContrasena(string contrasena, string hashGuardado)
        {
            return HashearContrasena(contrasena) == hashGuardado;
        }

        public string HashearContrasena(string contrasena)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(contrasena);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Estudiante)
                .Include(u => u.Profesor)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Estudiante)
                .Include(u => u.Profesor)
                .ToListAsync();
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
