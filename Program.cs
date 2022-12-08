
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;

Console.WriteLine("¡ADO.NET Y BASES DE DATOS!");

using IHost host = Host.CreateDefaultBuilder(args).Build();

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