using Microsoft.AspNetCore.SignalR;

namespace VMA_API.Application.Hubs
{
    public class ImportHub : Hub
    {
        public async Task SendProgress(int percentage, string fileName)
        {
            await Clients.All.SendAsync($"{fileName} - ReceiveProgress", percentage);
        }
    }
}
