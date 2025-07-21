using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public required string Correo { get; set; }
        public required string Programa { get; set; }
        public required string Jornada { get; set; }
        public int CreditosMaximos { get; set; } 
        public int CreditosUtilizados { get; set; }

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public List<EstudianteMateria> EstudianteMaterias { get; set; } = new();

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellidos}";
    }
}
