using gestionactivos.Models;
using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace gestionactivos.Data
{
    public class FuncionariosData
    {
        //poder obtner toda la lista de conctactos
        public List<FuncionarioModel> Listar()
        {
            var olist = new List<FuncionarioModel>();
            //instacion de conexion
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarF",conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using(var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //lectura de todo el resultado
                        olist.Add(new FuncionarioModel()
                        {
                            idFuncionario = Convert.ToInt32(dr["idFuncionario"]),
                            Cedula = dr["Cedula"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Area = dr["Area"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Piso = dr["Piso"].ToString()
                        });
                        }
                    }
                }
            return olist;
        }

        public FuncionarioModel Obtener(int idFuncionario)
        {
            var oContacto = new FuncionarioModel();
            //instacion de conexion
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ObtenerF", conexion);
                cmd.Parameters.AddWithValue("idFuncionario", idFuncionario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //lectura de todo el resultado
                        oContacto.idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
                        oContacto.Cedula = dr["Cedula"].ToString();
                        oContacto.Nombre = dr["Nombre"].ToString();
                        oContacto.Apellidos = dr["Apellidos"].ToString();
                        oContacto.Telefono = dr["Telefono"].ToString();
                        oContacto.Correo = dr["Correo"].ToString();
                        oContacto.Area = dr["Area"].ToString();
                        oContacto.Cargo = dr["Cargo"].ToString();
                        oContacto.Piso = dr["Piso"].ToString();
                        oContacto.idClientes = Convert.ToInt32(dr["idClientes"]);
                        oContacto.idSedes = Convert.ToInt32(dr["idSedes"]);
                    }
                }
            }
            return oContacto;
        }

        public (bool Resultado, string Mensaje) Guardar(FuncionarioModel oContacto, int idSede)
        {
            bool rspta = false; // Inicializar como false
            string error = string.Empty; // Variable para almacenar el mensaje de error

            try
            {
                // Instanciación de conexión
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_GuardarF", conexion);
                    cmd.Parameters.AddWithValue("Cedula", oContacto.Cedula);
                    cmd.Parameters.AddWithValue("Nombre", oContacto.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oContacto.Apellidos);
                    cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                    cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                    cmd.Parameters.AddWithValue("Area", oContacto.Area);
                    cmd.Parameters.AddWithValue("Cargo", oContacto.Cargo);
                    cmd.Parameters.AddWithValue("Piso", oContacto.Piso);
                    cmd.Parameters.AddWithValue("@SedeID", idSede);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery(); // Ejecutar el procedimiento almacenado
                }
                rspta = true;
                return (rspta, string.Empty);
            }
            catch (SqlException sqlEx)
            {
                error = sqlEx.Message; // Captura el mensaje de error
                rspta = false; // Establece rspta a false
                return (rspta, error); // Devuelve falso y el mensaje de error de SQL
            }
            catch (Exception ex)
            {
                error = ex.Message; // Captura el mensaje de error general
                rspta = false; // Establece rspta a false
                return (rspta, error); // Devuelve falso y el mensaje de error general
            }

        }

        public bool Editar(FuncionarioModel oContacto)
        {
            bool rspta;
            try
            {
                // Instancia de la conexión
                var cn = new Conexion();
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EditarF", conexion);
                    cmd.Parameters.AddWithValue("idFuncionario", oContacto.idFuncionario);
                    cmd.Parameters.AddWithValue("Cedula", oContacto.Cedula);
                    cmd.Parameters.AddWithValue("Nombre", oContacto.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oContacto.Apellidos);
                    cmd.Parameters.AddWithValue("Telefono", oContacto.Telefono);
                    cmd.Parameters.AddWithValue("Correo", oContacto.Correo);
                    cmd.Parameters.AddWithValue("Area", oContacto.Area);
                    cmd.Parameters.AddWithValue("Cargo", oContacto.Cargo);
                    cmd.Parameters.AddWithValue("Piso", oContacto.Piso);
                    cmd.Parameters.AddWithValue("idClientes", oContacto.idClientes); // Agregar el cliente
                    cmd.Parameters.AddWithValue("SedeID", oContacto.idSedes); // Agregar la sede
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

        public bool Eliminar(int idFuncionario)
        {
            bool rspta;
            try
            {
                //instacion de conexion
                var cn = new Conexion();

                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarF", conexion);
                    cmd.Parameters.AddWithValue("idFuncionario", idFuncionario);
                   
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


        public List<FuncionarioModel> ListarClientes()
        {
            var olistClientes = new List<FuncionarioModel>();
            //instacion de conexion
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarClientes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //lectura de todo el resultado
                        olistClientes.Add(new FuncionarioModel()
                        {
                            idClientes = Convert.ToInt32(dr["idClientes"]),
                            Clientes = dr["Clientes"].ToString(),
                            
                        });

                    }
                }
            }
            return olistClientes;
        }

        public List<FuncionarioModel> ListarSedes(int idClientes)
        {
            var olistSedes = new List<FuncionarioModel>();
            //instacion de conexion
            var cn = new Conexion();
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ListarSedes", conexion);
                cmd.Parameters.AddWithValue("@idClientes", idClientes);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        //lectura de todo el resultado
                        olistSedes.Add(new FuncionarioModel()
                        {
                            idSedes = Convert.ToInt32(dr["idSedes"]),
                            NombreSedes = dr["NombreSede"].ToString(),

                        });

                    }
                }
            }
            return olistSedes;
        }








    }
}

