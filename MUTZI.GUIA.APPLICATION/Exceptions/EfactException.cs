namespace MUTZI.GUIA.APPLICATION.Exceptions
{
    public class EfactException : Exception
    {
        // Constructor por defecto
        public EfactException() { }

        // Constructor con mensaje
        public EfactException(string message)
            : base(message) { }

        // Constructor con mensaje y excepción interna
        public EfactException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
