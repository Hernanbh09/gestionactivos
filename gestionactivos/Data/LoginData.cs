using gestionactivos.Data;
using gestionactivos.Error;
using gestionactivos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public LoginModel ConsultarUsuario(string Correo, string Clave)
    {
        var errorLogger = new ErrorLogger();
        LoginModel usuario = null;
        string hashClave = ObtenerHashSha256(Clave);

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "sp_Login"; // Nombre del procedimiento almacenado
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros...
                    SqlParameter correoParam = new SqlParameter("@Correo", SqlDbType.VarChar, 100) { Value = Correo };
                    SqlParameter claveParam = new SqlParameter("@Contrasena", SqlDbType.VarChar, 256) { Value = hashClave };
                    SqlParameter mensajeParam = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                    SqlParameter idUsuarioParam = new SqlParameter("@idUsuario", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter rolParam = new SqlParameter("@Rol", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    SqlParameter nombreCompletoParam = new SqlParameter("@NombreCompleto", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };

                    // Añadir parámetros al comando...
                    command.Parameters.AddRange(new[] { correoParam, claveParam, mensajeParam, idUsuarioParam, rolParam, nombreCompletoParam });

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
                    else
                    {
                        // Llamar al nuevo método para registrar el mensaje de error general
                        errorLogger.RegistrarError($"Correo: {Correo}, Mensaje: {mensaje}", "Clientes Metodo:" + nameof(ConsultarUsuario));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errorLogger.RegistrarError(ex, "Clientes Metodo:" + nameof(ConsultarUsuario));
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
