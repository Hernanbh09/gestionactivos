using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class LoginModel
    {

        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; }
        public string Rol{ get; set; }


        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
    }
    
}
