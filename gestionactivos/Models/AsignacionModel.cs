namespace gestionactivos.Models
{
    public class AsignacionModel
    {
        internal string Rol;

        public int idFuncionario { get; set; }
        public string? Cedula { get; set; }

        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Area { get; set; }
        public string? Cargo { get; set; }
        public string? Piso { get; set; }

        public string? Placa { get; set; }
        public int idArticulo { get; set; }
        public string? ArticuloCategoria { get; set; }
        public string? ArticuloModelo { get; set; }
        public string? ArticuloSerial { get; set; }
        public string? ArticuloPlaca { get; set; }
        public int? idAdicional { get; set; }
        public string? AdicionalCategoria { get; set; }
        public string? AdicionalModelo { get; set; }
        public string? AdicionalSerial { get; set; }
        public string? AdicionalPlaca { get; set; }
        // Campos para cedulaContra
        public int idFuncionarioContra { get; set; }
        public string? CedulaContra { get; set; }

        public string? NombresContra { get; set; }
        public string? ApellidosContra { get; set; }
        public string? CorreoContra { get; set; }

        //Resultados Adicional

        public string? Categoria { get; set; }
        public string? Modelo { get; set; }
        public string? Serial { get; set; }
        public object IdAsignacion { get; set; }
        public string idUsuario { get; internal set; }


        //Campos Extras


    }
}
