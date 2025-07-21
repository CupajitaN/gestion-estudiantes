namespace Backend.DTOs
{
    public class ProfesorMateriaDto
    {
        public int Id { get; set; }
        
        public string NombreMateria { get; set; } = string.Empty;
        public string NombreProfesor { get; set; } = string.Empty;

        public int ProfesorId { get; set; } 
        public int MateriaId { get; set; } 
    }
}