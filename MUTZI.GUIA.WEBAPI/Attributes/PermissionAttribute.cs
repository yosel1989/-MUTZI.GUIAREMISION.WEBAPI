namespace MUTZI.GUIA.WEBAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PermissionAttribute : Attribute
    {
        public string Codigo { get; }

        public PermissionAttribute(string codigo)
        {
            Codigo = codigo;
        }
    }
}
