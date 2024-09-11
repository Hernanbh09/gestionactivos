using gestionactivos.Models;
using gestionactivos.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

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
        public bool AgregarAdicional(int idArticulo, int idAdicional)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_AgregarAdicional"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idArticulo", idArticulo);
                    command.Parameters.AddWithValue("@idAdicional", idAdicional);

                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                        return true; // Si se ejecuta correctamente, devolvemos true
                    }
                    catch (Exception)
                    {
                        return false; // Si ocurre un error, devolvemos false
                    }
                }
            }
        }
        public bool GuardarFirmaFuncionario(int idFuncionario, string dataURL)
        {
            try
            {
                byte[] firmaBytes = Convert.FromBase64String(dataURL.Split(',')[1]); // Convertir Base64 a binario

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_GuardarFirmarFunci"; // Nombre del procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdFuncionario", idFuncionario);
                        command.Parameters.AddWithValue("@FirmaFuncionario", firmaBytes);

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
                byte[] firmaBytes = Convert.FromBase64String(dataURLR.Split(',')[1]); // Convertir Base64 a binario

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_GuardarFirmarFunci"; // Nombre del procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdFuncionario", idContratista);
                        command.Parameters.AddWithValue("@FirmaFuncionario", firmaBytes);

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

        public AsignacionModel AsignacionEquipoTerminada(int idFuncionario, int idArticulo, int idFuncionarioContra, int idUsuario)
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


        public string ObtenerDocumento(int idAsignacion)
        {
            DataTable resultTable = new DataTable();
            string jsonResult = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_CrearDocumento"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idAsignacion", idAsignacion);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                    try
                    {
                        connection.Open();
                        dataAdapter.Fill(resultTable);

                        // Convertir DataTable a JSON
                        jsonResult = JsonConvert.SerializeObject(resultTable);
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores, puedes registrar el error o lanzarlo según sea necesario
                        Console.WriteLine("Error al ejecutar el procedimiento almacenado: " + ex.Message);
                        // Puedes devolver un JSON vacío o null en caso de error
                        jsonResult = null;
                    }
                }
            }

            return jsonResult;
        }




    }
}
