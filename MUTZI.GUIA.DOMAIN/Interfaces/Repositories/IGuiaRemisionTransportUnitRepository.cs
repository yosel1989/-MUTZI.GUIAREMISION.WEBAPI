using MUTZI.GUIA.DOMAIN.Entities;

namespace MUTZI.GUIA.DOMAIN.Interfaces.Repositories
{
    public interface IGuiaRemisionTransportUnitRepository
    {
        Task<List<GuiaRemisionTransportUnit>> Create(List<GuiaRemisionTransportUnit> relation);
        Task<List<GuiaRemisionTransportUnit>> Update(List<GuiaRemisionTransportUnit> relation);
    }
}
