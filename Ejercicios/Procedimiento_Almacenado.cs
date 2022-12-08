using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet.Ejercicios {
    internal class Procedimiento_Almacenado {

        private async void procedimientoAlmacenado() {
            using IHost host = Host.CreateDefaultBuilder().Build();

            var configuration = host.Services.GetService<IConfiguration>();
            var conexion = configuration!.GetConnectionString("cadenaConexion");

            Console.Write("Escribe el nombre a agregar: ");
            var nombre = Console.ReadLine();

            try {
                using (SqlConnection connection = new SqlConnection(conexion)) {
                    /* Abrimos la conexión */
                    connection.Open();

                    Console.WriteLine($"Conexión Abierta");

                    var query = "Insertar_Persona";

                    using (SqlCommand command = new SqlCommand(query, connection)) {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@Nombre", nombre));

                        var paramId = new SqlParameter {
                            ParameterName = "@Id",
                            Direction = ParameterDirection.Output,
                            DbType = DbType.Int32
                        };

                        command.Parameters.Add(paramId);
                        await command.ExecuteNonQueryAsync();

                        var id = (int)paramId.Value;
                        Console.WriteLine($"Id de la persona: { id }");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"No se pudo abrir la conexión");
                Console.WriteLine($"Msg = { ex.Message }");
            }
        }

        private async void obteniendoScalar() {
            using IHost host = Host.CreateDefaultBuilder().Build();

            var configuration = host.Services.GetService<IConfiguration>();
            var conexion = configuration!.GetConnectionString("cadenaConexion");

            try {
                using (SqlConnection connection = new SqlConnection(conexion)) {
                    /* Abrimos la conexión */
                    connection.Open();

                    Console.WriteLine($"Conexión Abierta");

                    var query = @"SELECT COUNT(*)
                      FROM Personas";

                    using (SqlCommand command = new SqlCommand(query, connection)) {
                        var cantidad = await command.ExecuteScalarAsync();
                        Console.WriteLine($"Cantidad de registros: { cantidad }");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"No se pudo abrir la conexión");
                Console.WriteLine($"Msg = { ex.Message }");
            }
        }

        private async void dataTableAndSP() {
            using IHost host = Host.CreateDefaultBuilder().Build();

            var configuration = host.Services.GetService<IConfiguration>();
            var conexion = configuration!.GetConnectionString("cadenaConexion");

            try {
                using (SqlConnection connection = new SqlConnection(conexion)) {
                    /* Abrimos la conexión */
                    connection.Open();

                    Console.WriteLine($"Conexión Abierta\n");

                    var query = "Leer_Personas";

                    using (SqlCommand command = new SqlCommand(query, connection)) {
                        command.CommandType = CommandType.StoredProcedure;


                        using (SqlDataAdapter adaptador = new SqlDataAdapter(command)) {
                            var dt = new DataTable();

                            adaptador.Fill(dt);

                            foreach (DataRow fila in dt.Rows) {
                                Console.WriteLine($"{fila["Id"]} | {fila["Nombre"]}");
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"No se pudo abrir la conexión");
                Console.WriteLine($"Msg = { ex.Message }");
            }
        }
    }
}
