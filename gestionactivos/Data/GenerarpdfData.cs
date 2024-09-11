using gestionactivos.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace gestionactivos.Data
{
    public class GenerarpdfData
    {
        private string connectionString;
        public GenerarpdfData()
        {
            Conexion conexion = new Conexion();
            connectionString = conexion.getCadenaSQL();
        }

        public List<GenerarpdfModel> GenerarDocumento(int idAsignacion)
        {
            var asignaciones = new List<GenerarpdfModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_Crearpdf";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idAsignacion", idAsignacion);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var asignacion = new GenerarpdfModel
                        {
                            idAsignacion = reader.GetInt32(0),

                        };
                        asignaciones.Add(asignacion);
                    }

                    reader.Close();
                }
            }

            return asignaciones;
        }
    }
}
