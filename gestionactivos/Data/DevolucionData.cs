using gestionactivos.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public List<DevolucionModel> ConsultarCedula(string cedula)
        {
            List<DevolucionModel> funcionarios = new List<DevolucionModel>();

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
                        command.Parameters.AddWithValue("@cedula", cedula);

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
                                    idMovimientos = reader.IsDBNull(5) ? 0 : reader.GetInt32(11),
                                    idArticulo = reader.IsDBNull(11) ? 0 : reader.GetInt32(12),
                                    CategoriaArticulo = reader.IsDBNull(12) ? string.Empty : reader.GetString(13),
                                    ModeloArticulo = reader.IsDBNull(13) ? string.Empty : reader.GetString(14),
                                    SerialArticulo = reader.IsDBNull(14) ? string.Empty : reader.GetString(15),
                                    PlacaArticulo = reader.IsDBNull(15) ? string.Empty : reader.GetString(16),
                                    idAdicional = reader.IsDBNull(16) ? 0 : reader.GetInt32(17),
                                    CategoriaAdicional = reader.IsDBNull(17) ? string.Empty : reader.GetString(18),
                                    ModeloAdicional = reader.IsDBNull(18) ? string.Empty : reader.GetString(19),
                                    SerialAdicional = reader.IsDBNull(19) ? string.Empty : reader.GetString(20),
                                    PlacaAdicional = reader.IsDBNull(20) ? string.Empty : reader.GetString(21),
                                };
                                funcionarios.Add(funcionario);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejar excepciones relacionadas con SQL
                // Por ejemplo, registrar el error o lanzar una excepción personalizada
                throw new Exception("Error al consultar la cédula en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones
                throw new Exception("Error inesperado al consultar la cédula.", ex);
            }

            return funcionarios;
        }



    }
}
