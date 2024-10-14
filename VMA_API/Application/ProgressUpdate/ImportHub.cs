using Microsoft.AspNetCore.SignalR;

namespace VMA_API.Application.ProgressUpdate
{
    public class ImportHub : Hub
    {
        public async Task SendProgress(int percentage)
        {
            await Clients.All.SendAsync("ReceiveProgress", percentage);
        }
    }
}
