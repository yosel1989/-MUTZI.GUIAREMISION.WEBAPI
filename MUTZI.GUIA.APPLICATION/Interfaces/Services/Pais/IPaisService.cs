using MUTZI.GUIA.APPLICATION.DTOs.Pais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.Pais
{
    public interface IPaisService
    {
        IEnumerable<PaisesDTO> GetPaisesAsync();
    }
}
