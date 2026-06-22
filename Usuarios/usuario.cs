namespace Usuarios;

    // 1. Molde secundario: Representa el bloque "address" del JSON
    public class Domicilio
    {
        public string ?Street { get; set; }
        public string ?Suite { get; set; }
        public string ?City { get; set; }
    }

    // 2. Molde principal: Representa al usuario completo
    public class Usuario
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Email { get; set; }
        
        // Aquí conectamos las dos clases. El domicilio es un objeto complejo, no un string.
        public Domicilio ?Address { get; set; } 
    }
