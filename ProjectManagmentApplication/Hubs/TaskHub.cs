using Microsoft.AspNet.SignalR;

namespace ProjectManagmentApplication.Hubs
{
    public class TaskHub : Hub
    {
        //public void TaskUpdated(int taskId)
        //{
        //    Clients.All.taskUpdated(taskId);
        //}

        //public static void SendMessage(string msg)
        //{
        //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<TaskHub>();
        //    hubContext.Clients.All.foo(msg);
        //}
    }
}