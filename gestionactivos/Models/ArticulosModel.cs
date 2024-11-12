using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class ArticulosModel
    {
        public int idArticulo { get; set; }



        public int idCategoria { get; set; }

        public string? Categoria { get; set; }
        public string? Modelo { get; set; }



        [Required(ErrorMessage = "El campo Serial es obligatorio")]
        public string? Serial { get; set; }

        [Required(ErrorMessage = "El campo Placa es obligatorio")]
        public string? Placa { get; set; }

    }
}
