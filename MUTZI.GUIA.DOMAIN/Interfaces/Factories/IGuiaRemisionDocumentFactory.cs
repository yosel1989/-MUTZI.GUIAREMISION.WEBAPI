using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Document;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Factories
{
    public interface IGuiaRemisionDocumentFactory
    {
        IGuiaRemisionDocument Create(GuiaRemision entidad);
    }
}
