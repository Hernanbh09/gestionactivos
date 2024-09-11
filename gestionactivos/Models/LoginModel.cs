namespace gestionactivos.Models
{
    public class LoginModel
    {

        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; }
        public string Rol{ get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }


    }
}
