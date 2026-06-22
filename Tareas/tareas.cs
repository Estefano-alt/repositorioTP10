using System.Text.Json.Serialization;

namespace TareasApp // <-- El mismo nombre que uses en tu Program.cs
{
    public class Tarea
    {
        [JsonPropertyName("userId")]
        public int userID { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("title")]
        public string? titulo { get; set; }

        [JsonPropertyName("completed")]
        public bool completado { get; set; }
    }
}