using gestionactivos.Models;

namespace gestionactivos.Data
{
    public class EntregaData
    {
        public List<EntregaFuncionarioModel> Funcionario { get; set; } = new List<EntregaFuncionarioModel>();
        public List<EntregaArticuloModel> Articulo { get; set; } = new List<EntregaArticuloModel>();

    }

    public class EntregaFuncionarioModel
    {
        public int EncargadoIdFuncionario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public string Piso { get; set; }

    }

    public class EntregaArticuloModel
    {
        public int IdArticulo { get; set; }
        public string CategoriaArticulo { get; set; }
        public string ModeloArticulo { get; set; }
        public string SerialArticulo { get; set; }
        public string PlacaArticulo { get; set; }
        public List<EntregaAdicionalModel> Adicionales { get; set; } = new List<EntregaAdicionalModel>();
    }
    public class EntregaAdicionalModel
    {
        public int? IdAdicional { get; set; }
        public string CategoriaAdicional { get; set; }
        public string ModeloAdicional { get; set; }
        public string SerialAdicional { get; set; }
        public string PlacaAdicional { get; set; }
    }
}
