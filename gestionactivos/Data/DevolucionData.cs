using gestionactivos.Error;
using gestionactivos.Models;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace gestionactivos.Data
{
    public class DevolucionData
    {
        private string connectionString;


        public DevolucionData()
        {
            Conexion conexion = new Conexion();
            connectionString = conexion.getCadenaSQL();
        }

        public List<DevolucionModel> ConsultarCedula(string cedula, int? idUsuario)
        {
            List<DevolucionModel> funcionarios = new List<DevolucionModel>();
            var errorLogger = new ErrorLogger();

            // Validar la cédula antes de realizar la consulta
            if (string.IsNullOrWhiteSpace(cedula))
            {
                throw new ArgumentException("La cédula no puede estar vacía o contener solo espacios.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_ConsultarDevolucion";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@cedula", SqlDbType.VarChar).Value = cedula;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DevolucionModel funcionario = new DevolucionModel
                                {
                                    EncargadoIdFuncionario = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    EncargadoCedula = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                    EncargadoNombres = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    EncargadoApellidos = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                    EncargadoCorreo = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                    ResponsableIdFuncionario = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                    ResponsableCedula = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    ResponsableNombre = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                    ResponsableApellido = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                    ResponsableCorreo = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                    FechaMovimiento = reader.IsDBNull(10) ? (DateTime?)null : reader.GetDateTime(10),
                                    idMovimientos = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                                    idArticulo = reader.IsDBNull(12) ? 0 : reader.GetInt32(12),
                                    CategoriaArticulo = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                    ModeloArticulo = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                    SerialArticulo = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                                    PlacaArticulo = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
                                    idAdicional = reader.IsDBNull(17) ? 0 : reader.GetInt32(17),
                                    CategoriaAdicional = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                    ModeloAdicional = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                    SerialAdicional = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                    PlacaAdicional = reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                                };

                                funcionarios.Add(funcionario);
                                // Cambié Console.WriteLine por un logger adecuado
                                // logger.LogInformation($"ResponsableIdFuncionario: {funcionario.ResponsableIdFuncionario}");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                errorLogger.RegistrarError(ex, "Devolucion Metodo:" + nameof(ConsultarCedula), idUsuario);
                throw new Exception("Error al consultar la cédula en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Devolucion Metodo:" + nameof(ConsultarCedula), idUsuario);
                throw new Exception("Error inesperado al consultar la cédula.", ex);
            }

            return funcionarios;
        }



        public bool GuardarFirmaFuncionario( int CedulaFuncionario, string dataURL, int? idUsuario)
        {
            var errorLogger = new ErrorLogger();
            try
            {
                // Extraer la parte Base64 de la cadena Data URL
                string base64String = dataURL.Split(',')[1]; // Usa `dataURL` en lugar de `dataURLR`

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_GuardarFirmarFunci"; // Nombre del procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdFuncionario", CedulaFuncionario);
                        command.Parameters.AddWithValue("@FirmaFuncionario", base64String); // Pasar la cadena Base64 directamente

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true; // Si se ejecuta correctamente, devolvemos true
                    }
                }
            }
            catch (Exception ex)
            {
                // Registra el error (opcionalmente)
                errorLogger.RegistrarError(ex, "Devolucion Metodo:" + nameof(GuardarFirmaFuncionario), idUsuario);
                return false; // Si ocurre un error, devolvemos false
            }
        }
        //public bool GuardarFirmaContratista(int idContratista, string dataURLR)
        //{
        //    var errorLogger = new ErrorLogger();
        //    try
        //    {
        //        string base64String = dataURLR.Split(',')[1]; // Convertir Base64 a binario

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            string sqlQuery = "sp_GuardarFirmarFunci"; // Nombre del procedimiento almacenado
        //            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@IdFuncionario", idContratista);
        //                command.Parameters.AddWithValue("@FirmaFuncionario", base64String);

        //                connection.Open();
        //                command.ExecuteNonQuery();
        //                return true; // Si se ejecuta correctamente, devolvemos true
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Registra el error (opcionalmente)
        //        errorLogger.RegistrarError(ex, "Devolucion Metodo:" + nameof(GuardarFirmaContratista), idContratista);
        //        return false; // Si ocurre un error, devolvemos false
        //    }
        //}


        public DevolucionModel DevolucionEquipoTerminado(int idFuncionario, int idFuncionarioContra, int idArticulo, int idUsuario, int idMovimiento, string Observacion)
        {
            DevolucionModel devolucionEquipo = new DevolucionModel();
            var errorLogger = new ErrorLogger();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_RegistrarMovimiento";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        command.Parameters.AddWithValue("@idEvento", 2); // Devolución
                        command.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                        command.Parameters.AddWithValue("@idFuncionarioContra", idFuncionarioContra);
                        command.Parameters.AddWithValue("@idArticulo", idArticulo);
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        command.Parameters.AddWithValue("@Observacion", Observacion);
                        command.Parameters.AddWithValue("@idMovimiento", idMovimiento);

                        // Parámetro de salida para @idMovimientos
                        SqlParameter outputIdMovimientos = new SqlParameter("@idMovimientos", SqlDbType.Int);
                        outputIdMovimientos.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputIdMovimientos);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Recuperar el valor de salida @idMovimientos
                        if (outputIdMovimientos.Value != DBNull.Value)
                        {
                            // Almacena el idMovimientos en el modelo
                            devolucionEquipo.IdMovimientos = (int)outputIdMovimientos.Value; // Asegúrate de que la propiedad IdMovimientos exista en DevolucionModel
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex, "Devolucion Metodo:" + nameof(DevolucionEquipoTerminado), idUsuario);
            }
            return devolucionEquipo;
        }




    }
}
