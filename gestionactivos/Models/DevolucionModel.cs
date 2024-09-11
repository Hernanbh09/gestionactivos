using System.ComponentModel.DataAnnotations;

namespace gestionactivos.Models
{
    public class DevolucionModel
    {


        [Required(ErrorMessage = "Ingrese una cédula.")]
        public string Cedula { get; set; }

        public string Message { get; set; }

        public int EncargadoIdFuncionario { get; set; }
        public string EncargadoCedula { get; set; }
        public string EncargadoApellidos { get; set; }
        public string EncargadoNombres { get; set; }
        public string EncargadoCorreo { get; set; }
        public int ResponsableIdFuncionario { get; set; }
        public string ResponsableCedula { get; set; }
        public string ResponsableNombre { get; set; }
        public string ResponsableApellido { get; set; }
        public string ResponsableCorreo { get; set; }

        public int idMovimientos { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public int idArticulo { get; set; }
        public string CategoriaArticulo { get; set; }
        public string ModeloArticulo { get; set; }
        public string SerialArticulo { get; set; }
        public string PlacaArticulo { get; set; }
        public int idAdicional { get; set; }
        public string CategoriaAdicional { get; set; }
        public string ModeloAdicional { get; set; }
        public string SerialAdicional { get; set; }
        public string PlacaAdicional { get; set; }


    }
}
