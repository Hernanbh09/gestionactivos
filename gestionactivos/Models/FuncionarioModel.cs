using System.ComponentModel.DataAnnotations;


namespace gestionactivos.Models
{
    public class FuncionarioModel
    {

  


        public int idFuncionario { get; set; }

        [Required(ErrorMessage ="El campo Cedula es obligatorio")]
        public string? Cedula { get; set; }


        [Required(ErrorMessage ="El campo Nombre es obligatorio")]
        public string? Nombre { get; set; }


        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        public string? Apellidos { get; set; }


        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        public string? Telefono { get; set; }


        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        public string? Correo { get; set; }


        [Required(ErrorMessage = "El campo Area es obligatorio")]
        public string? Area { get; set; }


        [Required(ErrorMessage = "El campo Cargo es obligatorio")]
        public string? Cargo { get; set; }


        [Required(ErrorMessage = "El campo Piso es obligatorio")]
        public string? Piso { get; set; }

        public int? idClientes { get; set; }
        public string? Clientes { get; set; }
        public int? idSedes { get; set; }
        public string? NombreSedes { get; set; }
        public int? Estado { get; set; }


    }
}
 