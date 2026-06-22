namespace Tareas;

 public class Tarea
    {
        // Representa el "userId" (es un número entero)
        public int UserId { get; set; }

        // Representa el "id" de la tarea (es un número entero)
        public int Id { get; set; }

        // Representa el "title" de la tarea (es texto)
        public string ?Title { get; set; }

        // Representa si está "completed" o no (es verdadero/falso)
        public bool Completed { get; set; }
    }
   
