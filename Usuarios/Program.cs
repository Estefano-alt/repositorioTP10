using System;
using Usuarios;
using System.Net.Http;           // Para usar HttpClient
using System.Threading.Tasks;    // Para operaciones asíncronas
using System.Text.Json;          // Para Serializar y Deserializar
using System.Collections.Generic;// Para List<>
using System.Linq;               // Para usar la herramienta .Take()
using System.IO;                 // Para File.WriteAllText

namespace Usuarios
{
    class Program
    {
        //  Única instancia estática
        private static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            // El endpoint (URL) del Ejercicio 2
            string url = "https://jsonplaceholder.typicode.com/users/";

            try
            {
                Console.WriteLine("Conectando a la API de Usuarios...\n");

                // ==========================================================
                // 1. PETICIÓN GET
                // ==========================================================
                HttpResponseMessage response = await client.GetAsync(url);
                
                // Aseguramos que la respuesta fue exitosa (Código 200 OK)
                response.EnsureSuccessStatusCode();
                
                // Leemos el cuerpo del mensaje como un string gigante
                string responseBody = await response.Content.ReadAsStringAsync();


                // ==========================================================
                // 2. DESERIALIZACIÓN (JSON a C#)
                // ==========================================================
                var opcionesLectura = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                // Le agregamos el ?? new List<Usuario>() para evitar el warning de la línea ondulada
                List<Usuario> listaUsuarios = JsonSerializer.Deserialize<List<Usuario>>(responseBody, opcionesLectura) ?? new List<Usuario>();


                // ==========================================================
                // 3. MOSTRAR LOS PRIMEROS 5 USUARIOS
                // ==========================================================
                // La herramienta .Take(5) recorta la lista y nos devuelve solo los 5 primeros
                var primerosCinco = listaUsuarios.Take(5);

                Console.WriteLine("=== PRIMEROS 5 USUARIOS ===");
                foreach (var user in primerosCinco)
                {
                    // Mostramos nombre y correo
                    Console.WriteLine($"- Nombre: {user.Name}");
                    Console.WriteLine($"  Correo: {user.Email}");
                    
                    // Mostramos el domicilio accediendo al objeto anidado (user.Address...)
                    Console.WriteLine($"  Domicilio: {user.Address?.Street}, {user.Address?.Suite}, {user.Address?.City}\n");
                }


                // ==========================================================
                // 4. GUARDAR EN EL SISTEMA DE ARCHIVOS LOCAL
                // ==========================================================
                // Para guardar los datos localmente, volvemos a serializar la lista a un texto prolijo
                var opcionesEscritura = new JsonSerializerOptions { WriteIndented = true };
                string jsonFormateado = JsonSerializer.Serialize(listaUsuarios, opcionesEscritura);

                // Escribimos el archivo en el disco duro
                File.WriteAllText("usuarios.json", jsonFormateado);

                Console.WriteLine("¡Éxito! El listado completo se ha guardado localmente en 'usuarios.json'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al consumir la API: {ex.Message}");
            }
        }
    }
}