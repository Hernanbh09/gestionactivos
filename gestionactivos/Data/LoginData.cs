using gestionactivos.Data;
using gestionactivos.Models;
using Microsoft.AspNetCore.Mvc;
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
    [ValidateAntiForgeryToken]
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

                // Parámetro de entrada para Correo
                SqlParameter correoParam = new SqlParameter("@Correo", SqlDbType.VarChar, 100)
                {
                    Value = Correo
                };
                command.Parameters.Add(correoParam);

                // Parámetro de entrada para Clave (hash)
                SqlParameter claveParam = new SqlParameter("@Contrasena", SqlDbType.VarChar, 256)
                {
                    Value = hashClave
                };
                command.Parameters.Add(claveParam);

                // Parámetro de salida para Mensaje
                SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(mensajeParam);

                // Parámetro de salida para idUsuario
                SqlParameter idUsuarioParam = new SqlParameter("@idUsuario", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(idUsuarioParam);

                // Parámetro de salida para Rol
                SqlParameter rolParam = new SqlParameter("@Rol", SqlDbType.VarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(rolParam);

                // Parámetro de salida para NombreCompleto
                SqlParameter nombreCompletoParam = new SqlParameter("@NombreCompleto", SqlDbType.VarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(nombreCompletoParam);

                connection.Open();
                command.ExecuteNonQuery();

                // Obtenemos los valores de los parámetros de salida
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
                sb.Append(b.ToString("x2")); // Genera el hash en formato hexadecimal
            }
            return sb.ToString(); // Devuelve el hash como cadena
        }
    }
}
