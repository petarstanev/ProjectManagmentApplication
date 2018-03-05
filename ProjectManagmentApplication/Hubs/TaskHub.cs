using Microsoft.AspNet.SignalR;

namespace ProjectManagmentApplication.Hubs
{
    public class TaskHub : Hub
    {
        public void TaskUpdated(int taskId)
        {
            Clients.All.taskUpdated(taskId);
        }
    }
}