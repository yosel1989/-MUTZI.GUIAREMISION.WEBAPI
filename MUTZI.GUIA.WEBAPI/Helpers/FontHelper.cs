using QuestPDF.Drawing;

namespace MUTZI.GUIA.WEBAPI.Helpers
{
    public static class FontHelper
    {
        public static void RegisterFonts()
        {
            // Ruta relativa: carpeta Fonts dentro del proyecto
            FontManager.RegisterFontFromStream(File.OpenRead("Fonts/ARIAL.TTF")); // Regular
            FontManager.RegisterFontFromStream(File.OpenRead("Fonts/ARIALBD.TTF")); // Bold
            //FontManager.RegisterFontFromStream(File.OpenRead("Fonts/Arial-Italic.ttf"));   // Italic
            //FontManager.RegisterFontFromStream(File.OpenRead("Fonts/Arial-BoldItalic.ttf"));// Bold Italic
        }
    }
}
