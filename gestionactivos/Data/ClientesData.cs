using gestionactivos.Error;
using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims; // Asegúrate de agregar esto

namespace gestionactivos.Data
{
    public class ClientesData
    {
        public List<ClientesModel> Listar(int? idUsuario)
        {
            var olist = new List<ClientesModel>();
            var errorLogger = new ErrorLogger();

            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Clientes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetro de entrada
                    cmd.Parameters.AddWithValue("@Evento", 2);

                    // Parámetro de salida
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    // Ejecutar el procedimiento almacenado
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            olist.Add(new ClientesModel
                            {
                                idClientes = Convert.ToInt32(dr["idClientes"]),
                                Clientes = dr["Clientes"].ToString()
                            });
                        }
                    }
                    string mensaje = (string)cmd.Parameters["@MensajeCliente"].Value;
                }
            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Clientes Metodo:" + nameof(Listar), idUsuario);
            }
            return olist;
        }


        public bool Guardar(ClientesModel oClientes, out string MensajeError, int? idUsuario)
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
                    SqlCommand cmd = new SqlCommand("sp_Clientes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Evento", 1);
                    cmd.Parameters.AddWithValue("@Clientes", oClientes.Clientes);

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
                errorLogger.RegistrarError(ex, "Clientes Metodo:" + nameof(Guardar), idUsuario);
                MensajeError = "Ocurrió un error al intentar guardar el cliente.";
            }
            return rspta;
        }
        public ClientesModel Obtener(int idClientes, out string MensajeError, int? idUsuario)
        {
            var oClientes = new ClientesModel();
            MensajeError = string.Empty;
            var errorLogger = new ErrorLogger();

            try
            {
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_Clientes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@Evento", 5);
                    cmd.Parameters.AddWithValue("@idClientes", idClientes);

                    // Parámetro de salida para el mensaje
                    SqlParameter mensajeParam = new SqlParameter("@MensajeCliente", SqlDbType.VarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    // Ejecutar el procedimiento almacenado
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read()) // Cambiado para solo leer el primer resultado
                        {
                            oClientes.idClientes = Convert.ToInt32(dr["idClientes"]);
                            oClientes.Clientes = dr["Clientes"].ToString();
                        }
                    }

                    // Capturar el mensaje de salida
                    MensajeError = mensajeParam.Value.ToString();
                }
            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Clientes Metodo:" + nameof(Obtener), idUsuario);
                MensajeError = "Ocurrió un error al intentar obtener el cliente.";
            }

            return oClientes;
        }

        public bool Editar(ClientesModel oClientes, out string MensajeError, int? idUsuario)
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
                    SqlCommand cmd = new SqlCommand("sp_Clientes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Evento", 3);
                    cmd.Parameters.AddWithValue("@Clientes", oClientes.Clientes);
                    cmd.Parameters.AddWithValue("@idClientes", oClientes.idClientes); // Asegúrate de incluir el ID del cliente que se está editando

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

        public bool Eliminar(int idClientes, out string MensajeError, int? idUsuario)
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
                    SqlCommand cmd = new SqlCommand("sp_Clientes", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Evento", 4);
                    cmd.Parameters.AddWithValue("@idClientes", idClientes);

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
                errorLogger.RegistrarError(ex, "Clientes Metodo:" + nameof(Eliminar), idUsuario);
                MensajeError = "Ocurrió un error al eliminar el Cliente.";
                rspta = false;
            }
            return rspta;
        }

    }
}
