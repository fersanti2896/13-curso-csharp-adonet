
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        var query = @"INSERT INTO Personas(Nombre)
                      VALUES(@Nombre)";

        using (SqlCommand command = new SqlCommand(query, connection)) {
            command.Parameters.Add(new SqlParameter("@Nombre", nombre));

            var filas = await command.ExecuteNonQueryAsync();

            Console.WriteLine($"Filas afectadas: { filas }");
        }
    }
} catch (Exception ex) {
    Console.WriteLine($"No se pudo abrir la conexión");
    Console.WriteLine($"Msg = { ex.Message }");
}