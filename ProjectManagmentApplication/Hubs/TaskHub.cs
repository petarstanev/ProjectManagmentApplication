using Microsoft.AspNet.SignalR;

namespace ProjectManagmentApplication.Hubs
{
    public class TaskHub : Hub
    {
        public static void TaskUpdated(int taskid)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
            hubContext.Clients.All.taskUpdated(taskid);
        }
    }
}