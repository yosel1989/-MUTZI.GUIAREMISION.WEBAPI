namespace MUTZI.GUIA.APPLICATION.Helpers
{
    public class Helper
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static bool GuardarJSON(string json, string ruta)
        {
            RemoveJsonFile(ruta);
            File.WriteAllText(ruta, json);
            return true;

        }
        public static bool RemoveJsonFile(string ruta)
        {
            if (File.Exists(ruta))
                File.Delete(ruta);
            return true;
        }
    }
}
