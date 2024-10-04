using gestionactivos.Models;
using gestionactivos.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using iText.StyledXmlParser.Jsoup.Helper;

namespace gestionactivos.Data
{
    public class AsignacionData
    {

        private string connectionString;

        public AsignacionData()
        {
            // Obtener la cadena de conexión desde la clase Conexion
            Conexion conexion = new Conexion();
            connectionString = conexion.getCadenaSQL();
        }

        public AsignacionModel ConsultarCedula(string cedula)
        {

            AsignacionModel funcionario = new AsignacionModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_consultarCedulaF"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@cedula", cedula);


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Llenar el modelo AsignacionModel con los datos del funcionario
                            funcionario.idFuncionario = reader.GetInt32(0); // Suponiendo que el ID está en la primera columna
                            funcionario.Cedula = reader.GetString(1);
                            funcionario.Nombres = reader.GetString(2);
                            funcionario.Apellidos = reader.GetString(3);
                            funcionario.Correo = reader.GetString(4);
                            funcionario.Telefono = reader.GetString(5);
                            funcionario.Area = reader.GetString(6);
                            funcionario.Cargo = reader.GetString(7);
                            funcionario.Piso = reader.GetString(8);
                        }
                    }
                    else
                    {
                        // Si no se encuentran resultados, devolver null
                        funcionario = null;
                    }

                    reader.Close();
                }
            }
            return funcionario;
        }
        public List<AsignacionModel> ConsultarPlaca(string placa)
        {
            List<AsignacionModel> articulos = new List<AsignacionModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_ConsultarPlaca"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Placa", placa);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AsignacionModel articulo = new AsignacionModel
                        {
                            idArticulo = reader.GetInt32(0),
                            ArticuloCategoria = reader.GetString(1),
                            ArticuloModelo = reader.GetString(2),
                            ArticuloSerial = reader.GetString(3),
                            ArticuloPlaca = reader.GetString(4),
                            idAdicional = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            AdicionalCategoria = reader.IsDBNull(6) ? null : reader.GetString(6),
                            AdicionalModelo = reader.IsDBNull(7) ? null : reader.GetString(7),
                            AdicionalSerial = reader.IsDBNull(8) ? null : reader.GetString(8),
                            AdicionalPlaca = reader.IsDBNull(9) ? null : reader.GetString(9)
                        };
                        articulos.Add(articulo);
                    }
                    reader.Close();
                }
            }
            return articulos;
        }


        public AsignacionModel ConsultarCedulaContra(string cedulaContra)
        {
            AsignacionModel contratista = new AsignacionModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_consultarCedulaC"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Cedula", cedulaContra);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            contratista.idFuncionarioContra = reader.GetInt32(0);
                            contratista.NombresContra = reader.GetString(1);
                            contratista.ApellidosContra = reader.GetString(2);
                            contratista.CorreoContra = reader.GetString(3);
                        }
                    }
                    else
                    {
                        // Si no se encuentran resultados, devolver null
                        contratista = null;
                    }

                    reader.Close();
                }
            }
            return contratista;
        }


        public AsignacionModel ConsultarPlacaAdicional(string PlacaAdicional)
        {
            AsignacionModel adicionales = new AsignacionModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_ConsultarPlacaAdicional"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Placa", PlacaAdicional);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            adicionales.idAdicional = reader.GetInt32(0);
                            adicionales.Categoria = reader.GetString(1);
                            adicionales.Modelo = reader.GetString(2);
                            adicionales.Serial = reader.GetString(3);
                            adicionales.Placa = reader.GetString(3);
                        }
                    }
                    else
                    {
                        // Si no se encuentran resultados, devolver null
                        adicionales = null;
                    }

                    reader.Close();
                }
            }
            return adicionales;
        }
        public (bool Success, string Message) AgregarAdicional(int idArticulo, int idAdicional)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_AgregarAdicional"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idArticulo", idArticulo);
                    command.Parameters.AddWithValue("@idAdicional", idAdicional);

                    // Parámetro de salida para el mensaje
                    SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(mensajeParam);

                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                        string mensaje = mensajeParam.Value.ToString();
                        return (true, mensaje); // Mensaje de éxito o error
                    }
                    catch (Exception ex)
                    {
                        return (false, ex.Message); // Captura cualquier error
                    }
                }
            }
        }


        public bool GuardarFirmaFuncionario(int idFuncionario, string dataURL)
        {
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
                        command.Parameters.AddWithValue("@IdFuncionario", idFuncionario);
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
                Console.WriteLine(ex.Message);
                return false; // Si ocurre un error, devolvemos false
            }
        }
        public bool GuardarFirmaContratista(int idContratista, string dataURLR)
        {
            try
            {
                string base64String = dataURLR.Split(',')[1]; // Convertir Base64 a binario

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_GuardarFirmarFunci"; // Nombre del procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdFuncionario", idContratista);
                        command.Parameters.AddWithValue("@FirmaFuncionario", base64String);

                        connection.Open();
                        command.ExecuteNonQuery();
                        return true; // Si se ejecuta correctamente, devolvemos true
                    }
                }
            }
            catch (Exception ex)
            {
                // Registra el error (opcionalmente)
                Console.WriteLine(ex.Message);
                return false; // Si ocurre un error, devolvemos false
            }
        }

        public AsignacionModel AsignacionEquipoTerminada(int idFuncionario, int idArticulo, int idFuncionarioContra, int idUsuario, int? estadoCheckbox)
        {
            AsignacionModel AsignarEquipoTermi = new AsignacionModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_RegistrarMovimiento"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idFuncionario", idFuncionario);
                    command.Parameters.AddWithValue("@idArticulo", idArticulo);
                    command.Parameters.AddWithValue("@idFuncionarioContra", idFuncionarioContra);
                    command.Parameters.AddWithValue("@idEvento",1);
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    command.Parameters.AddWithValue("@estadoCheckbox", estadoCheckbox);

                    SqlParameter outputIdParam = new SqlParameter("@idMovimientos", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    AsignarEquipoTermi.IdAsignacion = (int)outputIdParam.Value;
                }
            }
            return AsignarEquipoTermi;
        }



        public (string CorreoEncargado, string CorreoResponsable) ObtenerCorreoPorAsignacion(int idAsignacion)
        {
            string correoEncargado = null;
            string correoResponsable = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_ConsultarCorreo"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idMovimiento", idAsignacion); // Asegúrate que sea el parámetro correcto

                    connection.Open(); // Asegúrate de abrir la conexión

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Aquí verificamos si los correos existen
                            correoEncargado = reader.IsDBNull(0) ? null : reader.GetString(0);
                            correoResponsable = reader.IsDBNull(1) ? null : reader.GetString(1);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron resultados para el idMovimiento: " + idAsignacion);
                    }

                    reader.Close();
                }
            }
            return (correoEncargado, correoResponsable);
        }




    }
}
