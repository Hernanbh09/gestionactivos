using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Data
{
    public class MovimientosData
    {

        public List<MovimientosModel> Listar()
        {
            var olist = new List<MovimientosModel>();
            var cn = new Conexion();

            using(var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ConsultarMovimiento", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //lectura de todo el resultado
                        olist.Add(new MovimientosModel()
                        {
                            idMovimientos = Convert.ToInt32(dr["idMovimientos"]),
                            Evento = dr["Evento"].ToString(),
                            NombreEncargado = dr["NombreEncargado"].ToString(),
                            NombreResponsable = dr["NombreResponsable"].ToString(),
                            Categoria = dr["Categoria"].ToString(),
                            Modelo = dr["Modelo"].ToString(),
                            Serial = dr["Serial"].ToString(),
                            Placa = dr["Placa"].ToString(),
                            Archivo = dr["Archivo"].ToString(),

                        });

                    }
                }
            }
            return olist;

        }


    }
}
