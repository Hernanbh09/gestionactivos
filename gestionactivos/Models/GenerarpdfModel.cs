
using NuGet.Configuration;

namespace gestionactivos.Models
{
    public class GenerarpdfModel
    {
        public int idAsignacion { get; set; }
        public string EventoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string ObservacionMovimiento { get; set; }
        public int IdArticulo { get; set; }
        public string CategoriaArticulo { get; set; }
        public string ModeloArticulo { get; set; }
        public string SerialArticulo { get; set; }
        public string PlacaArticulo { get; set; }
        public List<AdicionalModel> Adicionales { get; set; } = new List<AdicionalModel>();

        public string CedulaFuncionario { get; set; }
        public string NombreFuncionario { get; set; }
        public string ApellidoFuncionario { get; set; }
        public string TelefonoFuncionario { get; set; }
        public string AreaFuncionario { get; set; }
        public string CargoFuncionario { get; set; }
        public string PisoFuncionario { get; set; }
        public string FirmaFuncionario { get; set; }
        public string CedulaContratista { get; set; }
        public string NombreContratista { get; set; }
        public string ApellidoContratista { get; set; }
        public string TelefonoContratista { get; set; }
        public string AreaContratista { get; set; }
        public string CargoContratista { get; set; }
        public string PisoContratista { get; set; }
        public string FirmaContratista { get; set; }
        public string NombreSedeFuncionario { get; set; }
        public string DireccionSedeFuncionario { get; set; }
        public string CuidadSede { get; set; }
        public string NombreClienteFuncionario { get; set; }
        public string NitEmpresa { get; set; }
        public string RazonSocialEmpresa { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string FirmaUsuario { get; set; }

        public Boolean ExtraMaleta { get; set; }
        public Boolean ExtraGuaya { get; set; }
        public Boolean ExtraBase { get; set; }
        public Boolean ExtraCargador { get; set; }
        public Boolean ExtraPadMouse { get; set; }
        public Boolean ExtraDiadema { get; set; }
    }
    public class AdicionalModel
    {
        public int? IdAdicional { get; set; }
        public string CategoriaAdicional { get; set; }
        public string ModeloAdicional { get; set; }
        public string SerialAdicional { get; set; }
        public string PlacaAdicional { get; set; }
    }


    

}
