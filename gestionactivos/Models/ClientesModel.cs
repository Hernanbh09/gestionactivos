using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class ClientesModel
    {
        public int idClientes { get; set; }

        [Required(ErrorMessage = "El campo clientes es obligatorio")]
        public string? Clientes { get; set; }
    }
}
