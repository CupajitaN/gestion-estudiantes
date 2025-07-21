namespace Backend.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Correo { get; set; }
        public required string ContrasenaHash { get; set; }
        public required string Rol { get; set; }

        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }
        public int? ProfesorId { get; set; }
        public Profesor? Profesor { get; set; }  
    }

}
