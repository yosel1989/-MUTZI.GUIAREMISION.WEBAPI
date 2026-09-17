using MUTZI.GUIA.DOMAIN.Entities.Catalogo;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories.Catalogo
{
    public interface IInvoiceTypeRepository
    {
        Task<IEnumerable<InvoiceType>> GetToSelectAsync();

        Task<InvoiceType?> GetById(int invoiceTypeId, bool strict);
    }
}
