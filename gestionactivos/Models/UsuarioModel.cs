using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class UsuarioModel
    {

        public int idUsuario { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string? Nombre { get; set; }

        
        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string? Contrasena { get; set; }

        [Required(ErrorMessage = "El campo Cargo es obligatorio")]
        public string? Cargo { get; set; }

        [Required(ErrorMessage = "El campo Rol es obligatorio")]
        public string? Rol { get; set; }

        public string? ImagenBase64 { get; set; }
    }
}
