using K4os.Hash.xxHash;
using Microsoft.EntityFrameworkCore;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;
using MUTZI.GUIA.INFRASTRUCTURE.Persistence;

namespace MUTZI.GUIA.INFRASTRUCTURE.Repositories
{
    public class SecurityPersonRepository : ISecurityPersonRepository
    {
        private readonly LogisticaContext _context;
        public SecurityPersonRepository(LogisticaContext context)
        {
            _context = context;
        }

        public async Task<SecurityPerson?> CreateAsync(SecurityPerson newSecurityPerson)
        {
            await _context.SecurityPersons.AddAsync(newSecurityPerson);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(newSecurityPerson.SecurityPersonId, true);
        }

        public async Task<SecurityPerson?> UpdateAsync(SecurityPerson editSecurityPerson)
        {
            await _context.SaveChangesAsync();

            return await GetByIdAsync(editSecurityPerson.SecurityPersonId, true);
        }

        public async Task<SecurityPerson?> GetByIdAsync(int id, bool strict)
        {
            var role = await _context.SecurityPersons
                .FirstOrDefaultAsync();

            if (strict && role == null)
            {
                throw new KeyNotFoundException($"El personal de seguridad registrado no se encontró.");
            }

            return role;
        }

        public async Task<(List<SecurityPerson>, int)> GetListAsync(
            int pageNumber,
            int pageSize,
            string? search
        )
        {
            try
            {
                var query = _context.SecurityPersons
                .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(x =>
                        x.Persona.NumeroDocumento.Contains(search) ||
                        x.Persona.Nombre.Contains(search) ||
                        x.Persona.ApellidoPaterno.Contains(search) ||
                        x.Persona.ApellidoMaterno.Contains(search)
                    );
                }

                var totalRecords = await query.CountAsync();

                var securityPersons = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new SecurityPerson
                    {
                        SecurityPersonId = x.SecurityPersonId,
                        PersonId = x.PersonId,

                        Persona = new Persona
                        {
                            Nombre = x.Persona.Nombre,
                            ApellidoPaterno = x.Persona.ApellidoPaterno,
                            ApellidoMaterno = x.Persona.ApellidoMaterno,
                            NumeroDocumento = x.Persona.NumeroDocumento,
                            Cargo = x.Persona.Cargo
                        },

                        Active = x.Active,
                        CreatedAt = x.CreatedAt,
                        CreatedAtUser = x.CreatedAtUser,
                        CreatedAtUserName = x.CreatedAtUserName,
                        UpdatedAt = x.UpdatedAt,
                        UpdatedAtUser = x.UpdatedAtUser,
                        UpdatedAtUserName = x.UpdatedAtUserName
                    })
                    .AsNoTracking()
                    .ToListAsync();

                var securityPersonIds = securityPersons
                    .Select(x => x.SecurityPersonId)
                    .ToList();


                // Obtener las series de cada registro
                var series = await _context.SecurityPersonEntityBranchSeries
                    .Where(x => securityPersonIds.Contains(x.SecurityPersonId))
                    .Select(x => new SecurityPersonEntityBranchSerie
                    {
                        SecurityPersonId = x.SecurityPersonId,

                        EntityBranchSerie = new EntityBranchSerie
                        {
                            Serie = x.EntityBranchSerie.Serie,

                            EntityBranch = new EntityBranch
                            {
                                Entity = new Entity
                                {
                                    Name = x.EntityBranchSerie.EntityBranch.Entity.Name,
                                    DocumentNumber = x.EntityBranchSerie.EntityBranch.Entity.DocumentNumber
                                }
                            }
                        }
                    })
                    .AsNoTracking()
                    .ToListAsync();


                // Obtener los establecimientos de cada registro
                var entityBranches = await _context.SecurityPersonEntityBranchs
                    .Where(x => securityPersonIds.Contains(x.SecurityPersonId))
                    .Select(x => new SecurityPersonEntityBranch
                    {
                        SecurityPersonId = x.SecurityPersonId,

                        EntityBranch = new EntityBranch
                        {
                            Description = x.EntityBranch.Description,

                            Entity = new Entity
                            {
                                Name = x.EntityBranch.Entity.Name,
                                DocumentNumber = x.EntityBranch.Entity.DocumentNumber
                            }
                        }
                    })
                    .AsNoTracking()
                    .ToListAsync();

                // Obtener los motivos de traslados de cada registros
                var reasonForTransfers = await _context.SecurityPersonReasonForTransfers
                    .Where(x => securityPersonIds.Contains(x.SecurityPersonId))
                    .Select(x => new SecurityPersonReasonForTransfer
                    {
                        SecurityPersonId = x.SecurityPersonId,

                        ReasonForTransfer = new MotivoTraslado
                        {
                            Nombre = x.ReasonForTransfer.Nombre,
                            CodigoSunat = x.ReasonForTransfer.CodigoSunat
                        }
                    })
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var person in securityPersons)
                {
                    person.SecurityPersonEntityBranchSeries = series
                        .Where(x => x.SecurityPersonId == person.SecurityPersonId)
                        .ToList();

                    person.SecurityPersonEntityBranchs = entityBranches
                        .Where(x => x.SecurityPersonId == person.SecurityPersonId)
                        .ToList();

                    person.SecurityPersonReasonForTransfers = reasonForTransfers
                        .Where(x => x.SecurityPersonId == person.SecurityPersonId)
                        .ToList();
                }

                return (securityPersons, totalRecords);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
