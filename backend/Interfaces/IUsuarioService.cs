using Backend.Models;

namespace Backend.Interfaces
{
   public interface IUsuarioService
    {
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task<Usuario?> CrearUsuarioAsync(string correo, string contrasena, string rol, int? estudianteId, int? profesorId);
        bool VerificarContrasena(string contrasena, string hashGuardado);
        string HashearContrasena(string contrasena);
        Task<Usuario?> GetByIdAsync(int id);
        Task<List<Usuario>> GetAllAsync();
        Task<bool> EliminarAsync(int id);
    }
}