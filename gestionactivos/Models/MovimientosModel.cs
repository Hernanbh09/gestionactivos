﻿namespace gestionactivos.Models
{
    public class MovimientosModel
    {

        public int idMovimientos { get; set; }
        public string? Evento { get; set; }
        public string? NombreEncargado{ get; set; }
        public string? NombreResponsable { get; set; }
        public string? Categoria{ get; set; }
        public string? Modelo{ get; set; }
        public string? Placa { get; set; }
        public string? Serial { get; set; }
        public string? CategoriaAdicional { get; set; }
        public string? ModeloAdicional { get; set; }
        public string? PlacaAdicional { get; set; }
        public string? SerialAdicional { get; set; }
        public string? Archivo { get; set; }
        public string? FechaMovimiento { get; set; }


    }
}
