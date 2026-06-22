namespace MiWebApi
{
    // Clase para el objeto anidado de la calificación
    public class Rating
    {
        public double Rate { get; set; }
        public int Count { get; set; }
    }

    // Clase principal del producto de la tienda
    public class Producto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        
        // Conectamos el objeto anidado, usando el signo '?' por si algún producto no tiene calificación
        public Rating? Rating { get; set; }
    }
}