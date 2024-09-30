using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Data
{
    public class AdicionalesData
    {


        public List<AdicionalesModel> Listar()
        {

            var oList = new List<AdicionalesModel>();

            var cn = new Conexion();


            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarAd", conexion);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oList.Add(new AdicionalesModel()
                        {
                            idAdicional = Convert.ToInt32(dr["idAdicional"]),
                            Categoria = dr["Categoria"].ToString(),
                            Modelo = dr["Modelo"].ToString(),
                            Serial = dr["Serial"].ToString(),
                            Placa = dr["Placa"].ToString(),

                        });
                    }
                }
            }
            return oList;
        }

        public string Guardar(AdicionalesModel oAdicional)
        {
            string mensaje = null;
            try
            {

                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarAd", conexion);


                    cmd.Parameters.AddWithValue("Categoria", oAdicional.Categoria);
                    cmd.Parameters.AddWithValue("Modelo", oAdicional.Modelo);
                    cmd.Parameters.AddWithValue("Serial", oAdicional.Serial);
                    cmd.Parameters.AddWithValue("Placa", oAdicional.Placa);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return mensaje;
        }


        public AdicionalesModel Obtener(int idAdicional)
        {
            var oAdicional = new AdicionalesModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerAd", conexion);
                cmd.Parameters.AddWithValue("idAdicional", idAdicional);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oAdicional.idAdicional = Convert.ToInt32(dr["idAdicional"]);
                        oAdicional.Categoria = dr["Categoria"].ToString();
                        oAdicional.Modelo = dr["Modelo"].ToString();
                        oAdicional.Serial = dr["Serial"].ToString();
                        oAdicional.Placa = dr["Placa"].ToString();
                    }
                }
            }
            return oAdicional;
        }

        public bool Editar(AdicionalesModel oAdicional) 
        {
            bool rspta;
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EditarAd", conexion);
                    cmd.Parameters.AddWithValue("idAdicional", oAdicional.idAdicional);
                    cmd.Parameters.AddWithValue("Categoria", oAdicional.Categoria);
                    cmd.Parameters.AddWithValue("Modelo", oAdicional.Modelo);
                    cmd.Parameters.AddWithValue("Serial", oAdicional.Serial);
                    cmd.Parameters.AddWithValue("Placa", oAdicional.Placa);





                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                rspta = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                rspta = false;


            }
            return rspta;
        }


        public bool Eliminar(int idAdicional) 
        {
            bool rspta;
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarAd", conexion);
                    cmd.Parameters.AddWithValue("idAdicional", idAdicional);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                rspta = true;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                rspta = false;
            }

            return rspta;
        }


    }
}
