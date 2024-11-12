using gestionactivos.Error;
using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Data
{
    public class ArticulosData
    {

        public List<ArticulosModel> Listar(int? idUsuario)
        {

            var olist = new List<ArticulosModel>();
            //instacion de conexion
            var cn = new Conexion();
            var errorLogger = new ErrorLogger();
            try
            {
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
            }
            catch (Exception ex )
            {
                errorLogger.RegistrarError(ex, "Articulos Metodo:" + nameof(Listar), idUsuario);
            }

           
            return olist;
        }

        public List<ArticulosModel> ListarA()
        {
            var olist = new List<ArticulosModel>();
            var errorLogger = new ErrorLogger();
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_ListarCategoria", conexion);
                    cmd.Parameters.AddWithValue("TipoCategoria", "Articulo");
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Ejecutar el procedimiento almacenado
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            olist.Add(new ArticulosModel
                            {

                                idCategoria = Convert.ToInt32(dr["idCategoria"]),
                                Categoria = dr["Categoria"].ToString(),
                                Modelo = dr["Modelo"].ToString()
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Sede Metodo:" + nameof(ListarA));
            }
            return olist;
        }












        public string Guardar(ArticulosModel oArticulo, int? idUsuario)
        {
            string mensaje = null;
            var errorLogger = new ErrorLogger();
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarA", conexion);

                    //cmd.Parameters.AddWithValue("Categoria", oArticulo.Categoria);
                    //cmd.Parameters.AddWithValue("Modelo", oArticulo.Modelo);
                    cmd.Parameters.AddWithValue("idCategoria", oArticulo.idCategoria);

                    cmd.Parameters.AddWithValue("Serial", oArticulo.Serial);
                    cmd.Parameters.AddWithValue("Placa", oArticulo.Placa);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                mensaje = ex.Message;  // Capturar el mensaje de error SQL
                errorLogger.RegistrarError(ex, "Articulos Metodo:" + nameof(Guardar), idUsuario);
            }
            return mensaje;
        }

        public ArticulosModel Obtener(int idArticulo, int? idUsuario)
        {
            var oArticulos = new ArticulosModel();
            var cn = new Conexion();
            var errorLogger = new ErrorLogger();

            try
            {
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
                            oArticulos.idCategoria = Convert.ToInt32(dr["idCategoria"]);
                            oArticulos.Categoria = dr["Categoria"].ToString();
                            oArticulos.Modelo = dr["Modelo"].ToString();
                            oArticulos.Serial = dr["Serial"].ToString();
                            oArticulos.Placa = dr["Placa"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex )
            {

                errorLogger.RegistrarError(ex, "Articulos Metodo:" + nameof(Obtener), idUsuario);
            }


           
            return oArticulos;
        }

        public bool Editar(ArticulosModel oArticulo, int? idUsuario)
        {
            bool rspta;
            var errorLogger = new ErrorLogger();
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EditarA", conexion);
                    cmd.Parameters.AddWithValue("idArticulo", oArticulo.idArticulo);
                    cmd.Parameters.AddWithValue("idCategoria", oArticulo.idCategoria);

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
                errorLogger.RegistrarError(ex, "Articulos Metodo:" + nameof(Editar), idUsuario);

            }
            return rspta;

        }

        public bool Eliminar(int idArticulo, int? idUsuario)
        {
            bool rspta;
            var errorLogger = new ErrorLogger();
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
                errorLogger.RegistrarError(ex, "Articulos Metodo:" + nameof(Eliminar), idUsuario);
            }

            return rspta;
        }

    }
}
