
namespace Backend.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public int Creditos { get; set; } = 3;

        public List<EstudianteMateria> EstudianteMaterias { get; set; } = new();
        public List<ProfesorMateria> ProfesorMaterias { get; set; } = new();
        
    }
}