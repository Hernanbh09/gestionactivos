using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class SedesModel
    {


        public int idSedes { get; set; }
        public int idClientes { get; set; }
        public string? ClienteConcatenado { get; set; }

        [Required(ErrorMessage = "El campo NombreSede es obligatorio")]
        public string? NombreSede { get; set; }


        [Required(ErrorMessage = "El campo DireccionSede es obligatorio")]
        public string? DireccionSede { get; set; }


        [Required(ErrorMessage = "El campo CuidadSede es obligatorio")]
        public string? CuidadSede  { get; set; }



        public string? Cliente { get; set; }

    }
}
