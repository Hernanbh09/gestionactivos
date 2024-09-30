using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class ArticulosModel
    {
        public int idArticulo { get; set; }

        [Required(ErrorMessage = "El campo Categoria es obligatorio")]
        public string? Categoria { get; set; }

        [Required(ErrorMessage = "El campo Modelo es obligatorio")]
        public string? Modelo { get; set; }

        [Required(ErrorMessage = "El campo Serial es obligatorio")]
        public string? Serial { get; set; }

        [Required(ErrorMessage = "El campo Placa es obligatorio")]
        public string? Placa { get; set; }

    }
}
