using gestionactivos.Error;
using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Data
{
    public class SedesData
    {

        public List<SedesModel> Listar(int? idUsuario)
        {
            var olist = new List<SedesModel>();
            var errorLogger = new ErrorLogger();
            try
            {
                var cn = new Conexion();
                using (var  conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Sedes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Evento", 1);

                    // Parámetro de salida
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    // Ejecutar el procedimiento almacenado
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            olist.Add(new SedesModel
                            {
                                ClienteConcatenado = dr["ClienteConcatenado"].ToString(),
                                idSedes = Convert.ToInt32(dr["idSedes"]),
                                idClientes = Convert.ToInt32(dr["idClientes"]),
                                NombreSede = dr["NombreSede"].ToString(),
                                DireccionSede = dr["DireccionSede"].ToString(),
                                CuidadSede = dr["CuidadSede"].ToString()
                            });
                        }
                    }
                    string mensaje = (string)cmd.Parameters["@MensajeCliente"].Value;
                }

            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Sede Metodo:" + nameof(Listar), idUsuario);
            }
            return olist;
        }


        public List<SedesModel> ListarC()
        {
            var olist = new List<SedesModel>();
            var errorLogger = new ErrorLogger();
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Sedes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Evento", 6);

                    // Parámetro de salida
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    // Ejecutar el procedimiento almacenado
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            olist.Add(new SedesModel
                            {

                                idClientes = Convert.ToInt32(dr["idClientes"]),
                                Cliente = dr["Clientes"].ToString()
                            });
                            }
                    }
                    string mensaje = (string)cmd.Parameters["@MensajeCliente"].Value;
                }

            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Sede Metodo:" + nameof(ListarC));
            }
            return olist;
        }


        public bool Guardar(SedesModel oSedes, out string MensajeError, int? idUsuario)
        {
            MensajeError = string.Empty;
            var errorLogger = new ErrorLogger();
            bool rspta = false;
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Sedes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Evento", 2);
                    cmd.Parameters.AddWithValue("@idClientes", oSedes.idClientes);
                    cmd.Parameters.AddWithValue("@NombreSede", oSedes.NombreSede);
                    cmd.Parameters.AddWithValue("@DireccionSede", oSedes.DireccionSede);
                    cmd.Parameters.AddWithValue("@CuidadSede", oSedes.CuidadSede);
                    // Parámetro de salida para el mensaje
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado

                    // Capturar el valor del parámetro de salida
                    MensajeError = mensajeParam.Value.ToString();
                    rspta = MensajeError.Contains("correctamente");

                }
            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Sede Metodo:" + nameof(Guardar), idUsuario);
                MensajeError = "Ocurrió un error al intentar guardar la sede.";
            }
            return rspta;
        }


        public SedesModel Obtener(int idSedes, out string MensajeError, int? idUsuario)
        {
            var oSede = new SedesModel();
            MensajeError = string.Empty;
            var errorLogger = new ErrorLogger();

            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Sedes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Evento", 3);
                    cmd.Parameters.AddWithValue("@idSedes", idSedes);

                    // Parámetro de salida para el mensaje
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);
                    // Ejecutar el procedimiento almacenado
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read()) // Cambiado para solo leer el primer resultado
                        {
                            oSede.idSedes = Convert.ToInt32(dr["idSedes"]);
                            oSede.idClientes = Convert.ToInt32(dr["idClientes"]);
                            oSede.NombreSede = dr["NombreSede"].ToString();
                            oSede.DireccionSede = dr["DireccionSede"].ToString();
                            oSede.CuidadSede = dr["CuidadSede"].ToString();
                        }
                    }

                    // Capturar el mensaje de salida
                    MensajeError = mensajeParam.Value.ToString();
                }

            }
            catch (Exception ex)
            {

                errorLogger.RegistrarError(ex, "Sede Metodo:" + nameof(Obtener), idUsuario);
                MensajeError = "Ocurrió un error al intentar obtener el cliente.";
            }
            return oSede;
        }

        public bool Editar(SedesModel oSedes, out string MensajeError, int? idUsuario)
        {
            bool rspta = false;
            MensajeError = string.Empty;
            var errorLogger = new ErrorLogger();
            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Sedes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Evento", 4);
                    cmd.Parameters.AddWithValue("@idSedes", oSedes.idSedes);
                    cmd.Parameters.AddWithValue("@idClientes", oSedes.idClientes);

                    cmd.Parameters.AddWithValue("@NombreSede", oSedes.NombreSede);
                    cmd.Parameters.AddWithValue("@DireccionSede", oSedes.DireccionSede);
                    cmd.Parameters.AddWithValue("@CuidadSede", oSedes.CuidadSede);
                    // Parámetro de salida para el mensaje
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    // Capturar el mensaje de salida
                    MensajeError = mensajeParam.Value.ToString();
                    rspta = true; // Se asume que si no hay excepción, la operación fue exitosa
                }
            }
            catch (Exception ex)
            {

                errorLogger.RegistrarError(ex, "Clientes Metodo:" + nameof(Editar), idUsuario);
                MensajeError = "Ocurrió un error al editar el Cliente.";
            }
            return rspta;
        }



        public bool Eliminar(int idSedes, out string MensajeError, int? idUsuario)
        {
            bool rspta = false;
            MensajeError = string.Empty;
            var errorLogger = new ErrorLogger();
            try
            {
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Sedes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Evento", 5);
                    cmd.Parameters.AddWithValue("@idSedes", idSedes);

                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    cmd.ExecuteNonQuery();

                    MensajeError = mensajeParam.Value.ToString();
                    rspta = MensajeError.Contains("Cliente eliminado correctamente");
                    rspta = true;
                }

            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Sedes Metodo:" + nameof(Eliminar), idUsuario);
                MensajeError = "Ocurrió un error al eliminar el Sedes.";
                rspta = false;
            }
            return rspta;
        }





    }
}
