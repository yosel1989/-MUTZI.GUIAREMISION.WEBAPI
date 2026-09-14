using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Document;
using MUTZI.GUIA.DOMAIN.Interfaces.Factories;
using MUTZI.GUIA.INFRASTRUCTURE.Document;

namespace MUTZI.GUIA.INFRASTRUCTURE.Factories
{
    public class GuiaRemisionDocumentFactory: IGuiaRemisionDocumentFactory
    {
        public IGuiaRemisionDocument Create(GuiaRemision guiaRemision)
        {
            return new GuiaRemisionDocument(guiaRemision);
        }
    }
}
