using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class SecurityPersonReasonForTransferRepository : ISecurityPersonReasonForTransferRepository
    {
        private readonly LogisticaContext _context;
        public SecurityPersonReasonForTransferRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(SecurityPersonReasonForTransfer newData)
        {
            try
            {
                await _context.SecurityPersonReasonForTransfers.AddAsync(newData);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("security_person__reason_for_transfer.PRIMARY"))
                {
                    throw new Exception("El personal ya tiene el motivo de traslado asignado.");
                }
                throw;
            }
            
        }

        public async Task<bool> DeleteAsync(SecurityPersonReasonForTransfer model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(List<SecurityPersonReasonForTransfer>, int)> GetListAsync(int securityPersonId, int pageNumber, int pageSize, string? search)
        {
            var query = _context.SecurityPersonReasonForTransfers
                .Where(x => x.SecurityPersonId == securityPersonId)
                .AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.ReasonForTransfer.Nombre.Contains(search)
                );
            }
            var totalRecords = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SecurityPersonReasonForTransfer
                {
                    SecurityPersonId = x.SecurityPersonId,
                    ReasonForTransferId = x.ReasonForTransferId,
                    CreatedAt = x.CreatedAt,
                    CreatedAtUser = x.CreatedAtUser,
                    CreatedAtUserName = x.CreatedAtUserName,
                    ReasonForTransfer = new MotivoTraslado
                    {
                        Nombre = x.ReasonForTransfer.Nombre,
                        CodigoSunat = x.ReasonForTransfer.CodigoSunat
                    }
                })
                .ToListAsync();
            return (data, totalRecords);

        }


        public async Task<List<int>> GetAllReasonForTransferIdsByAssigned(int employeeId)
        {
            var data = _context.SecurityPersonReasonForTransfers
                .Where(x => x.SecurityPerson.PersonId == employeeId)
                .Select(x => x.ReasonForTransfer.MotivoTrasladoId)
                .Distinct()
                .ToList();

            return data;
        }


        public async Task<List<SecurityPersonReasonForTransfer>> GetListByAssignedAsync(int employeeId)
        {
            try
            {
                var data = await _context.SecurityPersonReasonForTransfers
                .Where(x =>
                    x.SecurityPerson.Persona.PersonaId == employeeId &&
                    x.SecurityPerson.Active &&
                    x.ReasonForTransfer.Active
                )
                .Select(x => new SecurityPersonReasonForTransfer
                {
                    SecurityPersonId = x.SecurityPersonId,
                    ReasonForTransferId = x.ReasonForTransferId,
                    CreatedAt = x.CreatedAt,
                    CreatedAtUser = x.CreatedAtUser,
                    CreatedAtUserName = x.CreatedAtUserName,
                    ReasonForTransfer = new MotivoTraslado
                    {
                        MotivoTrasladoId = x.ReasonForTransfer.MotivoTrasladoId,
                        Nombre = x.ReasonForTransfer.Nombre,
                        CodigoSunat = x.ReasonForTransfer.CodigoSunat
                    }
                })
                .ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<int>> GetAllReasonIdsByAssigned(int employeeId)
        {
            var data = _context.SecurityPersonReasonForTransfers
                .Where(x => x.SecurityPerson.PersonId == employeeId)
                .Select(x => x.ReasonForTransferId)
                .Distinct()
                .ToList();

            return data;
        }


        // Validators

        public async Task<bool> HasAssigned(int employeeId, int reasonForTransferId)
        {
            var data = await _context.SecurityPersonReasonForTransfers
                .Where(x => x.SecurityPerson.PersonId == employeeId && x.ReasonForTransferId == reasonForTransferId)
                .AnyAsync();

            return data;
        }

    }
}
