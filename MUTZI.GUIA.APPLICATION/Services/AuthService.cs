using Microsoft.AspNetCore.Http;
using MUTZI.GUIA.APPLICATION.DTOs.User;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using System.Security.Claims;

public class AuthService: IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public UserJwtDTO CurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity.IsAuthenticated)
            return null;

        var Roles = user?.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .ToArray();

        var Id = user.FindFirst("Id")?.Value;
        var FirstName = user.FindFirst("FirstName")?.Value;
        var LastName = user.FindFirst("LastName")?.Value;
        var UserName = user.FindFirst("UserName")?.Value;
        var EmployeeId = user.FindFirst("EmployeeId")?.Value;
        var LocalTypeId = user.FindFirst("LocalTypeId")?.Value;
        var LocalId = user.FindFirst("LocalId")?.Value;
        var AreaId = user.FindFirst("AreaId")?.Value;
        var UserLocalId = user.FindFirst("UserLocalId")?.Value;
        var UserLocalDesc = user.FindFirst("UserLocalDesc")?.Value;
        var UserLocalId2 = user.FindFirst("UserLocalId2")?.Value;
        var UserLocalId3 = user.FindFirst("UserLocalId3")?.Value;
        var UserLocalDesc3 = user.FindFirst("UserLocalDesc3")?.Value;
        var UserLocalComercialId = user.FindFirst("UserLocalComercialId")?.Value;
        var IdUsuarioSN = user.FindFirst("IdUsuarioSN")?.Value;
        var ProfileAppId = user.FindAll("ProfileAppId");
        var RucEmpresa = user.FindFirst("RucEmpresa")?.Value;

        var perfiles = ProfileAppId?
            .Select(p =>
            {
                var parts = p.Value.Split(',');
                return new PerfilJWTDto
                {
                    Id = int.Parse(parts[0]),
                    IdApp = int.Parse(parts[1])
                };
            })
            .ToList();

        return new UserJwtDTO
        {
            Id = Id != null ? Convert.ToInt32(Id) : 0,
            FirstName = FirstName,
            LastName = LastName,
            UserName = UserName,
            EmployeeId = EmployeeId != null ? Convert.ToInt32(EmployeeId) : 0,
            LocalTypeId = LocalTypeId != null ? Convert.ToInt32(LocalTypeId) : 0,
            LocalId = LocalId != null ? Convert.ToInt32(LocalId) : 0,
            AreaId = AreaId != null ? Convert.ToInt32(AreaId) : 0,
            UserLocalId = UserLocalId != null ? Convert.ToInt32(UserLocalId) : 0,
            UserLocalDesc = UserLocalDesc != null ? Convert.ToString(UserLocalDesc) : null,
            UserLocalId2 = UserLocalId2 != null ? Convert.ToInt32(UserLocalId2) : 0,
            UserLocalId3 = UserLocalId3 != null ? Convert.ToInt32(UserLocalId3) : 0,
            UserLocalDesc3 = UserLocalDesc3 != null ? Convert.ToString(UserLocalDesc3) : null,
            UserLocalComercialId = UserLocalComercialId != null ? Convert.ToInt32(UserLocalComercialId) : 0,
            IdUsuarioSN = IdUsuarioSN != null ? Convert.ToInt32(IdUsuarioSN) : 0,
            Profiles = perfiles ?? new List<PerfilJWTDto>(),
            Roles = Roles.ToList(),
            RucEmpresa = RucEmpresa
        };
    }

}
