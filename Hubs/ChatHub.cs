using System.Threading.Tasks;
using NeHauar.Hubs.Clients;
using NeHauar.Models;
using Microsoft.AspNetCore.SignalR;

namespace NeHauar.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}