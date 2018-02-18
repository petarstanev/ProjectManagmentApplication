using Microsoft.AspNet.SignalR;

namespace ProjectManagmentApplication.Hubs
{
    public class BoardHub : Hub
    {
        public void BoardUpdated(int boardId)
        {
            Clients.All.boardUpdated(boardId);
        }
    }
}