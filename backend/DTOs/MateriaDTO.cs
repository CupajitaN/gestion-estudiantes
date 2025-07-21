namespace Backend.DTOs
{
    public class MateriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Profesor { get; set; }
    }
}