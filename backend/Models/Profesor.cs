using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Profesor
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Correo { get; set; }
        public required string Jornada { get; set; }
        public int MateriasMax { get; set; }

        public List<ProfesorMateria> ProfesorMaterias { get; set; } = new();
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
}