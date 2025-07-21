using System.Collections.Generic;

namespace Backend.DTOs
{
    public class ProfesorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public List<MateriaDTO> Materias { get; set; } = new();
    }
}