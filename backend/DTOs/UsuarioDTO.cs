using System.Collections.Generic;

namespace Backend.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public int? EstudianteId { get; set; }
        public int? ProfesorId { get; set; }
    }
}