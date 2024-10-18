using gestionactivos.Models;
using System.Data.SqlClient;
using System.Data;
using gestionactivos.Error;

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
            GenerarpdfModel currentAsignacion = null;
            var errorLogger = new ErrorLogger();
            try
            {
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
                            int idArticulo = reader.GetInt32(reader.GetOrdinal("IdArticulo"));

                            if (currentAsignacion == null || currentAsignacion.IdArticulo != idArticulo)
                            {
                                // Guardar la asignación anterior si existe.
                                if (currentAsignacion != null)
                                {
                                    asignaciones.Add(currentAsignacion);
                                }

                                // Crear una nueva asignación.
                                currentAsignacion = new GenerarpdfModel
                                {
                                    idAsignacion = reader.GetInt32(reader.GetOrdinal("idMovimiento")),
                                    EventoMovimiento = reader.GetString(reader.GetOrdinal("EventoMovimiento")),
                                    FechaMovimiento = reader.GetDateTime(reader.GetOrdinal("FechaMovimiento")),
                                    ObservacionMovimiento = reader.IsDBNull(reader.GetOrdinal("ObservacionMovimiento")) ? null : reader.GetString(reader.GetOrdinal("ObservacionMovimiento")),

                                    IdArticulo = idArticulo,
                                    CategoriaArticulo = reader.GetString(reader.GetOrdinal("CategoriaArticulo")),
                                    ModeloArticulo = reader.GetString(reader.GetOrdinal("ModeloArticulo")),
                                    SerialArticulo = reader.GetString(reader.GetOrdinal("SerialArticulo")),
                                    PlacaArticulo = reader.GetString(reader.GetOrdinal("PlacaArticulo")),

                                    CedulaFuncionario = reader.GetString(reader.GetOrdinal("CedulaFuncionario")),
                                    NombreFuncionario = reader.GetString(reader.GetOrdinal("NombreFuncionario")),
                                    ApellidoFuncionario = reader.GetString(reader.GetOrdinal("ApellidoFuncionario")),
                                    TelefonoFuncionario = reader.GetString(reader.GetOrdinal("TelefonoFuncionario")),
                                    AreaFuncionario = reader.GetString(reader.GetOrdinal("AreaFuncionario")),
                                    CargoFuncionario = reader.GetString(reader.GetOrdinal("CargoFuncionario")),
                                    PisoFuncionario = reader.GetString(reader.GetOrdinal("PisoFuncionario")),
                                    FirmaFuncionario = reader.IsDBNull(reader.GetOrdinal("FirmaFuncionario")) ? null : reader.GetString(reader.GetOrdinal("FirmaFuncionario")),
                                    CedulaContratista = reader.IsDBNull(reader.GetOrdinal("CedulaContratista")) ? null : reader.GetString(reader.GetOrdinal("CedulaContratista")),
                                    NombreContratista = reader.IsDBNull(reader.GetOrdinal("NombreContratista")) ? null : reader.GetString(reader.GetOrdinal("NombreContratista")),
                                    ApellidoContratista = reader.IsDBNull(reader.GetOrdinal("ApellidoContratista")) ? null : reader.GetString(reader.GetOrdinal("ApellidoContratista")),
                                    TelefonoContratista = reader.IsDBNull(reader.GetOrdinal("TelefonoContratista")) ? null : reader.GetString(reader.GetOrdinal("TelefonoContratista")),
                                    AreaContratista = reader.IsDBNull(reader.GetOrdinal("AreaContratista")) ? null : reader.GetString(reader.GetOrdinal("AreaContratista")),
                                    CargoContratista = reader.IsDBNull(reader.GetOrdinal("CargoContratista")) ? null : reader.GetString(reader.GetOrdinal("CargoContratista")),
                                    PisoContratista = reader.IsDBNull(reader.GetOrdinal("PisoContratista")) ? null : reader.GetString(reader.GetOrdinal("PisoContratista")),
                                    FirmaContratista = reader.IsDBNull(reader.GetOrdinal("FirmaContratista")) ? null : reader.GetString(reader.GetOrdinal("FirmaContratista")),

                                    NombreSedeFuncionario = reader.GetString(reader.GetOrdinal("NombreSedeFuncionario")),
                                    DireccionSedeFuncionario = reader.GetString(reader.GetOrdinal("DireccionSedeFuncionario")),
                                    CuidadSede = reader.GetString(reader.GetOrdinal("CuidadSede")),
                                    NombreClienteFuncionario = reader.GetString(reader.GetOrdinal("NombreClienteFuncionario")),

                                    NitEmpresa = reader.GetString(reader.GetOrdinal("NitEmpresa")),
                                    RazonSocialEmpresa = reader.GetString(reader.GetOrdinal("RazonSocialEmpresa")),
                                    TelefonoEmpresa = reader.GetString(reader.GetOrdinal("TelefonoEmpresa")),
                                    DireccionEmpresa = reader.GetString(reader.GetOrdinal("DireccionEmpresa")),
                                    NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                                    ApellidoUsuario = reader.GetString(reader.GetOrdinal("ApellidoUsuario")),
                                    FirmaUsuario = reader.IsDBNull(reader.GetOrdinal("FirmaUsuario")) ? null : reader.GetString(reader.GetOrdinal("FirmaUsuario"))
                                };
                            }

                            // Agregar adicional a la lista de adicionales.
                            var adicional = new AdicionalModel
                            {
                                IdAdicional = reader.IsDBNull(reader.GetOrdinal("idAdicional")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("idAdicional")),
                                CategoriaAdicional = reader.GetString(reader.GetOrdinal("CategoriaAdicional")),
                                ModeloAdicional = reader.GetString(reader.GetOrdinal("ModeloAdicional")),
                                SerialAdicional = reader.GetString(reader.GetOrdinal("SerialAdicional")),
                                PlacaAdicional = reader.GetString(reader.GetOrdinal("PlacaAdicional"))
                            };

                            currentAsignacion.Adicionales.Add(adicional);
                        }
                        reader.Close();

                        // Agregar el último currentAsignacion a la lista
                        if (currentAsignacion != null)
                        {
                            asignaciones.Add(currentAsignacion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                errorLogger.RegistrarError(ex, "Generar PDF Metodo:" + nameof(GenerarDocumento), idAsignacion);
            }

           

            return asignaciones;
        }


        public int Guardarpdf(int idasignacion, string rutaArchivo, string nombrepdf, string pdfBase64)
        {
            var errorLogger = new ErrorLogger();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "sp_Guardarpdf";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idMovimiento", idasignacion);
                        command.Parameters.AddWithValue("@rutaArchivo", rutaArchivo);
                        command.Parameters.AddWithValue("@nombreArchivo", nombrepdf);
                        command.Parameters.AddWithValue("@Archivo", pdfBase64);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Registra el error (opcionalmente)
                errorLogger.RegistrarError(ex, "Generar PDF Metodo:" + nameof(Guardarpdf), idasignacion);
            }

            return 1; 
        }




    }
}
