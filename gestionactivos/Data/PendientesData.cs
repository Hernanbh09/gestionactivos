using gestionactivos.Models;
using System.Data;
using System.Data.SqlClient;

namespace gestionactivos.Data
{
    public class PendientesData
    {

        private string connectionString;
        public PendientesData()
        {
            Conexion conexion = new Conexion();
            connectionString = conexion.getCadenaSQL();
        }
        public List<PendientesModel> Listar()
        {
            var olist = new List<PendientesModel>();
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("sp_ConsultarPendientes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        olist.Add(new PendientesModel()
                        {
                            idMovimientos = Convert.ToInt32(dr["idMovimientos"]),
                            idFuncionarioResponsable = Convert.ToInt32(dr["idFuncionarioResponsable"]),
                            Evento = dr["Evento"].ToString(),
                            FechaMovimiento = Convert.ToDateTime(dr["FechaMovimiento"]), // Leer la fecha del movimiento
                            Cedula = dr["Cedula"].ToString(), // Leer la cédula del funcionario
                            NombreCompleto = dr["NombreCompleto"].ToString(), // Leer el nombre completo concatenado
                            Placa = dr["Placa"].ToString(), // Leer la placa del artículo
                            Serial = dr["Serial"].ToString() // Leer el serial del artículo
                        });
                    }
                }
            }
            return olist;
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


        public bool ActualizaPendiente(int idMovimiento)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_ActualizaPendientes"; // Nombre del procedimiento almacenado
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idMovimiento", idMovimiento);

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

}
}
