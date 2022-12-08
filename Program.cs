
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

Console.WriteLine("¡ADO.NET Y BASES DE DATOS!");

using IHost host = Host.CreateDefaultBuilder(args).Build();

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