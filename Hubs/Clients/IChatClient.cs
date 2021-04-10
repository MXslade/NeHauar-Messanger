using System.Threading.Tasks;
using NeHauar.Models;

namespace NeHauar.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}