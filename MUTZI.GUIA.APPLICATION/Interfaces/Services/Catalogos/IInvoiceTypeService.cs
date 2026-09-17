using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos
{
    public interface IInvoiceTypeService
    {
        Task<IEnumerable<InvoiceTypeToSelectDTO>> GetInvoiceTypeToSelect();
    }
}
