namespace gestionactivos.Models
{
    public class PendientesModel
    {
        public int idMovimientos { get; set; }
        public int idFuncionarioResponsable { get; set; }
        public string Evento { get; set; }
        public DateTime FechaMovimiento { get; set; } // Agregar campo FechaMovimiento
        public string Cedula { get; set; } // Agregar campo Cedula
        public string NombreCompleto { get; set; } // Agregar campo NombreCompleto (F.Nombre + F.Apellidos)
        public string Placa { get; set; } // Agregar campo Placa de Articulo
        public string Serial { get; set; } // Agregar campo Serial de Articulo
    }
}
