using Microsoft.AspNetCore.SignalR;

namespace PasteleriaAPI.Hubs
{
    public class PedidoHub : Hub
    {
        public async Task SuscribirAPedidos()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "CanalPedidos");
        }

        public async Task DesuscribirDePedidos()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "CanalPedidos");
        }
    }
}
