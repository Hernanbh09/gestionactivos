using gestionactivos.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;
using gestionactivos.Error;

namespace gestionactivos.Data
{
    public class UsuariosData
    {


        //poder obtner toda la lista de conctactos
        public List<UsuarioModel> Listar(int? idUsuario)
        {
            var olist = new List<UsuarioModel>();
            //instacion de conexion
            var cn = new Conexion();

            var errorLogger = new ErrorLogger();

            try
            {
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_ListarU", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //lectura de todo el resultado
                            olist.Add(new UsuarioModel()
                            {
                                idUsuario = Convert.ToInt32(dr["idUsuario"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Contrasena = "**********",
                                Cargo = dr["Cargo"].ToString(),
                                Rol = dr["Rol"].ToString()
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                errorLogger.RegistrarError(ex, "Usuarios Metodo:" + nameof(Listar), idUsuario);
            }
           
            return olist;
        }


        public bool Guardar(UsuarioModel oUsuario, out string mensajeError, int? idUsuario)
        {
            mensajeError = string.Empty; // Inicializar el mensaje de error
            bool rspta;
            var errorLogger = new ErrorLogger();
            try
            {
                //instanciación de conexión
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarU", conexion);

                    // Hash the password before adding it to the command parameters
                    string hashedPassword = GenerateSHA256Hash(oUsuario.Contrasena);

                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oUsuario.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("Contrasena", hashedPassword); // Use the hashed password
                    cmd.Parameters.AddWithValue("Cargo", oUsuario.Cargo);
                    cmd.Parameters.AddWithValue("Rol", oUsuario.Rol); // Aquí ya es string
                    cmd.Parameters.AddWithValue("ImagenBase64", oUsuario.ImagenBase64 ?? (object)DBNull.Value);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Ejecutar el comando y capturar el resultado
                    cmd.ExecuteNonQuery();
                }
                rspta = true;
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message; // Capturar el mensaje de error sin validar el tipo
                rspta = false;
                errorLogger.RegistrarError(ex, "Usuarios Metodo:" + nameof(Guardar), idUsuario);
            }

            return rspta;
        }


        private string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public UsuarioModel Obtener(int idUsuario)
        {
            var oUsuario = new UsuarioModel();
            var errorLogger = new ErrorLogger();
            //instacion de conexion
            var cn = new Conexion();

            try
            {
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_ObtenerU", conexion);
                    cmd.Parameters.AddWithValue("idUsuario", idUsuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oUsuario.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                            oUsuario.Nombre = dr["Nombre"].ToString();
                            oUsuario.Apellidos = dr["Apellidos"].ToString();
                            oUsuario.Correo = dr["Correo"].ToString();
                            oUsuario.Contrasena = dr["Contrasena"].ToString();
                            oUsuario.Cargo = dr["Cargo"].ToString();
                            oUsuario.Rol = dr["Rol"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                errorLogger.RegistrarError(ex, "Usuarios Metodo:" + nameof(Obtener), idUsuario);
            }

          
            return oUsuario;

        }

        public bool Editar(UsuarioModel oUsuario)
        {
            bool rspta;

            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EditarU", conexion);
                    cmd.Parameters.AddWithValue("idUsuario", oUsuario.idUsuario);
                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oUsuario.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);

                    // Hash the password before adding it to the command parameters
                    string hashedPassword = GenerateSHA256Hash(oUsuario.Contrasena);
                    cmd.Parameters.AddWithValue("Contrasena", hashedPassword);
                    cmd.Parameters.AddWithValue("Cargo", oUsuario.Cargo);
                    cmd.Parameters.AddWithValue("Rol", oUsuario.Rol);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                rspta = true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                rspta = false;


            }
            return rspta;
        }
        public bool Eliminar(int idUsuario)
        {
            bool rspta;
            var errorLogger = new ErrorLogger();
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarU", conexion);
                    cmd.Parameters.AddWithValue("idUsuario", idUsuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                rspta = true;
                
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                rspta = false;
                errorLogger.RegistrarError(ex, "Usuarios Metodo:" + nameof(Eliminar), idUsuario);
            }

            return rspta;
            }
        }


    }

