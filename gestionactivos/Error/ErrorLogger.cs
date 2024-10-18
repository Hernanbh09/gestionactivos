using gestionactivos.Data;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Error
{
    public class ErrorLogger
    {
        private string connectionString;

        public ErrorLogger()
        {
            // Obtener la cadena de conexión desde la clase Conexion
            Conexion conexion = new Conexion();
            connectionString = conexion.getCadenaSQL();
        }

        public void RegistrarError(Exception ex, string metodo, int? idUsuario = null)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_InsertErrorLog", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros para el procedimiento almacenado
                    cmd.Parameters.AddWithValue("@Mensaje", ex.Message);
                    cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario ?? (object)DBNull.Value); // Si es nulo, se pasa como DBNull
                    cmd.Parameters.AddWithValue("@Metodo", metodo);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        // Nuevo método para registrar mensajes de error generales
        public void RegistrarError(string mensaje, string metodo, int? idUsuario = null)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                conexion.Open();
                using (var cmd = new SqlCommand("sp_InsertErrorLog", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros para el procedimiento almacenado
                    cmd.Parameters.AddWithValue("@Mensaje", mensaje);
                    cmd.Parameters.AddWithValue("@StackTrace", (object)DBNull.Value); // No hay stack trace en este caso
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario ?? (object)DBNull.Value); // Si es nulo, se pasa como DBNull
                    cmd.Parameters.AddWithValue("@Metodo", metodo);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
