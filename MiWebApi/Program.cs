using System;
using System.Net.Http;           
using System.Threading.Tasks;    
using System.Text.Json;          
using System.Collections.Generic;
using System.Linq;               
using System.IO;                 

namespace MiWebApi
{
    class Program
    {
        // 1. Instancia estática única recomendada por la teoría
        private static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            // URL de nuestra tienda virtual de prueba
            string url = "https://fakestoreapi.com/products";

            try
            {
                Console.WriteLine("Conectando a la tienda virtual...\n");

                // --- Petición GET ---
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); 
                string responseBody = await response.Content.ReadAsStringAsync();

                // --- Deserialización ---
                var opcionesLectura = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                
                // Convertimos el JSON en una lista de Productos
                List<Producto> listaProductos = JsonSerializer.Deserialize<List<Producto>>(responseBody, opcionesLectura) ?? new List<Producto>();

                // --- Mostrar información en pantalla ---
                // Vamos a mostrar solo los primeros 5 productos para que quede prolijo
                var primerosProductos = listaProductos.Take(5);

                Console.WriteLine("=== CATÁLOGO DE PRODUCTOS DESTACADOS ===");
                foreach (var prod in primerosProductos)
                {
                    // Mostramos los datos relevantes del e-commerce
                    Console.WriteLine($"- {prod.Title}");
                    Console.WriteLine($"  Categoría: {prod.Category} | Precio: ${prod.Price}");
                    // Usamos el '?' de navegación segura por si el Rating es nulo
                    Console.WriteLine($"  Calificación: {prod.Rating?.Rate} estrellas (Basado en {prod.Rating?.Count} reseñas)\n");
                }

                // --- Guardar en archivo local ---
                var opcionesEscritura = new JsonSerializerOptions { WriteIndented = true };
                string jsonFormateado = JsonSerializer.Serialize(listaProductos, opcionesEscritura);

                // Guardamos el catálogo completo en un archivo JSON local
                File.WriteAllText("catalogo_tienda.json", jsonFormateado);

                Console.WriteLine("¡Éxito! El catálogo completo ha sido guardado en 'catalogo_tienda.json'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error de conexión con la API: {ex.Message}");
            }
        }
    }
}