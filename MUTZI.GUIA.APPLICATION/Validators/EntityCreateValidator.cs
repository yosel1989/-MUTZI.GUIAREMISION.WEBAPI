using FluentValidation;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;
using MUTZI.GUIA.DOMAIN.Interfaces.Repositories;

namespace MUTZI.GUIA.APPLICATION.Validators
{
    public class EntityCreateValidator : AbstractValidator<EntityCreateDTO>
    {
        private readonly IEntityRepository _entityRepository;

        public EntityCreateValidator(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;

            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Role)
                .MustAsync(async (request, value, _) => !await _entityRepository.HasManyRole(request.DocumentNumber, value))
                    .WithMessage(request => request.Role.ToLower() switch
                    {
                        "proveedor" => "La entidad ya se encuentra registrada en la lista de proveedores.",
                        "cliente" => "La entidad ya se encuentra registrada en la lista de clientes.",
                        "transportista" => "La entidad ya se encuentra registrada en la lista de transportistas",
                        "emisor" => "La entidad ya se encuentra registrada en la lista de emisores.",
                        _ => $"La entidad ya se encuentra registrada."
                    });

            RuleFor(x => x.DocumentNumber)
                .MustAsync(async (value, _) => !await _entityRepository.HasManyDocumentNumber(value))
                    .WithMessage(request => $"La entidad con el número de documento {request.DocumentNumber} ya se encuentra registrada.");
        }
    }
}
