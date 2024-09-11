using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

public class LoginData
{
    private string connectionString;

    public LoginData()
    {
        Conexion conexion = new Conexion();
        connectionString = conexion.getCadenaSQL();
    }

    public LoginModel ConsultarUsuario(string Correo, string Clave)
    {
        LoginModel usuario = null;
        string hashClave = ObtenerHashSha256(Clave);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = "sp_Login"; // Nombre del procedimiento almacenado
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Correo", Correo);
                command.Parameters.AddWithValue("@Contrasena", hashClave);

                SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(mensajeParam);

                SqlParameter idUsuarioParam = new SqlParameter("@idUsuario", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(idUsuarioParam);

                SqlParameter rolParam = new SqlParameter("@Rol", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(rolParam);

                SqlParameter nombreCompletoParam = new SqlParameter("@NombreCompleto", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(nombreCompletoParam);

                connection.Open();
                command.ExecuteNonQuery();

                string mensaje = (string)mensajeParam.Value;
                if (!string.IsNullOrEmpty(mensaje) && mensaje.Contains("Inicio de sesión exitoso"))
                {
                    usuario = new LoginModel
                    {
                        IdUsuario = (int)idUsuarioParam.Value,
                        Rol = (string)rolParam.Value,
                        NombreCompleto = (string)nombreCompletoParam.Value
                    };
                }
            }
        }

        return usuario;
    }

    private string ObtenerHashSha256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
