using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Data
{
    public class ArticulosData
    {

        public List<ArticulosModel> Listar()
        {

            var olist = new List<ArticulosModel>();
            //instacion de conexion
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarA", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //lectura de todo el resultado
                        olist.Add(new ArticulosModel()
                        {
                            idArticulo = Convert.ToInt32(dr["idArticulo"]),
                            Categoria = dr["Categoria"].ToString(),
                            Modelo = dr["Modelo"].ToString(),
                            Serial = dr["Serial"].ToString(),
                            Placa = dr["Placa"].ToString(),

                        });

                    }
                }
            }
            return olist;
        }

        public string Guardar(ArticulosModel oArticulo)
        {
            string mensaje = null;
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarA", conexion);

                    cmd.Parameters.AddWithValue("Categoria", oArticulo.Categoria);
                    cmd.Parameters.AddWithValue("Modelo", oArticulo.Modelo);
                    cmd.Parameters.AddWithValue("Serial", oArticulo.Serial);
                    cmd.Parameters.AddWithValue("Placa", oArticulo.Placa);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;  // Capturar el mensaje de error SQL
            }
            return mensaje;
        }

        public ArticulosModel Obtener(int idArticulo)
        {
            var oArticulos = new ArticulosModel();
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerA", conexion);
                cmd.Parameters.AddWithValue("idArticulo", idArticulo);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oArticulos.idArticulo = Convert.ToInt32(dr["idArticulo"]);
                        oArticulos.Categoria = dr["Categoria"].ToString();
                        oArticulos.Modelo = dr["Modelo"].ToString();
                        oArticulos.Serial = dr["Serial"].ToString();
                        oArticulos.Placa = dr["Placa"].ToString();
                    }
                }
            }
            return oArticulos;
        }

        public bool Editar(ArticulosModel oArticulo)
        {
            bool rspta;
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EditarA", conexion);
                    cmd.Parameters.AddWithValue("idArticulo", oArticulo.idArticulo);
                    cmd.Parameters.AddWithValue("Categoria", oArticulo.Categoria);
                    cmd.Parameters.AddWithValue("Modelo", oArticulo.Modelo);
                    cmd.Parameters.AddWithValue("Serial", oArticulo.Serial);
                    cmd.Parameters.AddWithValue("Placa", oArticulo.Placa);

                   



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

        public bool Eliminar(int idArticulo)
        {
            bool rspta;
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarA", conexion);
                    cmd.Parameters.AddWithValue("idArticulo", idArticulo);
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
