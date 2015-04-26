using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

public class BrainWavesHub : Hub
{
    public override Task OnConnected()
    {
        string userName = Context.User.Identity.Name;
        string connectionId = Context.ConnectionId;

        //var user = Users.GetOrAdd(userName, _ => new User
        //{
        //    Name = userName,
        //    ConnectionIds = new HashSet<string>()
        //});

        //lock (user.ConnectionIds)
        //{

        //    user.ConnectionIds.Add(connectionId);

        //    // TODO: Broadcast the connected user
        //}

        return base.OnConnected();
    }

    public void Send(int concentrationLevel)
    {
        Clients.All.UpdateConcentrationLevel(concentrationLevel);
    }
}