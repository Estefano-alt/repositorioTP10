using Tareas;
using System;
using System.Net.Http;           // Para usar HttpClient y conectarnos a internet
using System.Threading.Tasks;    // Para usar programación asíncrona (async / await)
using System.Text.Json;          // Para traducir entre texto JSON y objetos C# (Serializar/Deserializar)
using System.Collections.Generic;// Para usar List<>
using System.Linq;               // Para poder usar la herramienta ".Where()" y filtrar las listas fácilmente
using System.IO;                 // Para manejar archivos físicos (guardar el .json en el disco duro)

namespace Tareas
{
    class Program
    {
        // Creamos una única instancia del "navegador" (HttpClient) para no saturar la memoria
        private static readonly HttpClient client = new HttpClient();

        // El Main debe ser 'async Task' para poder usar 'await' y esperar las respuestas de internet
        static async Task Main()
        {
            // La URL (endpoint) de la API que nos da las tareas
            string url = "https://jsonplaceholder.typicode.com/todos/";

            try
            {
                Console.WriteLine("Conectando a la API de Tareas...\n");

                // ==========================================================
                // PASO 1 y 2: CONEXIÓN A LA API Y OBTENCIÓN DE DATOS
                // ==========================================================
                
                // Pedimos los datos y "esperamos" (await) a que lleguen
                HttpResponseMessage response = await client.GetAsync(url);
                
                // Chequeamos que no haya errores (como un error 404 de página no encontrada)
                response.EnsureSuccessStatusCode();
                
                // Leemos toda la respuesta y la guardamos cruda en una variable de texto (string)
                string responseBody = await response.Content.ReadAsStringAsync();


                // ==========================================================
                // PASO 3: DESERIALIZACIÓN (Texto JSON -> Objetos C#)
                // ==========================================================
                
                // Le decimos al traductor que ignore si en el JSON dice "title" y en C# dice "Title"
                var opcionesLectura = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                // Traducimos el texto gigante en una Lista real de objetos de tu clase "Tarea"
                List<Tarea> listaTareas = JsonSerializer.Deserialize<List<Tarea>>(responseBody, opcionesLectura) ?? new List<Tarea>();


                // ==========================================================
                // PASO 4: RECORRER, FILTRAR Y MOSTRAR EN CONSOLA
                // ==========================================================
                
                // Usamos LINQ (.Where) para crear una sub-lista solo con las tareas pendientes (Completed == false)
                var tareasPendientes = listaTareas.Where(tarea => tarea.Completed == false);
                
                // Usamos LINQ (.Where) para crear otra sub-lista solo con las completadas (Completed == true)
                var tareasCompletadas = listaTareas.Where(tarea => tarea.Completed == true);

                Console.WriteLine("----- TAREAS PENDIENTES -----");
                foreach (var tarea in tareasPendientes)
                {
                    // Imprimimos el título y el estado de cada tarea pendiente
                    Console.WriteLine($"[PENDIENTE] {tarea.Title}");
                }

                Console.WriteLine("\n----- TAREAS COMPLETADAS -----");
                foreach (var tarea in tareasCompletadas)
                {
                    // Imprimimos el título y el estado de cada tarea completada
                    Console.WriteLine($"[COMPLETADA] {tarea.Title}");
                }


                // ==========================================================
                // PASO 5: SERIALIZACIÓN Y GUARDADO LOCAL (Objetos C# -> Archivo JSON)
                // ==========================================================
                
                // Configuramos las opciones de escritura para que el JSON se vea prolijo (con saltos de línea y sangrías)
                var opcionesEscritura = new JsonSerializerOptions { WriteIndented = true };

                // Tomamos nuestra lista completa de C# y la convertimos de nuevo a un texto JSON ordenado
                string jsonFormateado = JsonSerializer.Serialize(listaTareas, opcionesEscritura);

                // Guardamos ese texto físicamente en el disco duro con el nombre "tareas.json"
                File.WriteAllText("tareas.json", jsonFormateado);

                Console.WriteLine("\n¡Proceso finalizado! Se ha creado el archivo 'tareas.json' en tu directorio.");
            }
            catch (Exception ex)
            {
                // Si algo falla en la red o en el guardado, capturamos el error aquí
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }
        }
    }
}

