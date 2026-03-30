using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.Hubs
{
    public class GuiasHub: Hub
    {
        public async Task SuscribirseAGuias(string rucDestinatario)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, rucDestinatario);
        }
    }
}
