﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.SignalR;

public class BaseHub : Hub
{
    protected readonly ILifetimeScope _sl;

    public BaseHub(ILifetimeScope lifetimeScope)
    {
        _sl = lifetimeScope.BeginLifetimeScope();
        
    }

    protected ConcurrentDictionary<string, SignalRUser> _users
    {
        get
        {
            var app = Context.Request.GetHttpContext().Application;

            var users = app["_signalR_users"];
            if (users == null)
                app["_signalR_users"] = new ConcurrentDictionary<string, SignalRUser>();

            return (ConcurrentDictionary<string, SignalRUser>)app["_signalR_users"];
        }
    }

    public override Task OnConnected()
    {
        var userName = Context.User.Identity.Name;
        var connectionId = Context.ConnectionId;

        var user = _users.GetOrAdd(userName, _ => new SignalRUser
        {
            Name = userName,
            ConnectionIds = new HashSet<string>()
        });

        lock (user.ConnectionIds)
            user.ConnectionIds.Add(connectionId);

        if (user.ConnectionIds.Count == 1)
            Clients.All.UserConnected(userName);

        return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
        var userName = Context.User.Identity.Name;
        var connectionId = Context.ConnectionId;

        SignalRUser user;
        _users.TryGetValue(userName, out user);

        if (user != null)
        {
            lock (user.ConnectionIds)
            {
                user.ConnectionIds.RemoveWhere(connId => connId.Equals(connectionId));

                if (!user.ConnectionIds.Any())
                {
                    SignalRUser removedUser;
                    _users.TryRemove(userName, out removedUser);
                }
            }
        }

        return base.OnDisconnected(stopCalled);
    }

    public virtual IEnumerable<string> GetConnectedUsers()
    {
        return _users.Where(keyValuePair =>
        {
            lock (keyValuePair.Value.ConnectionIds)
            {
                return !keyValuePair.Value.ConnectionIds.Contains(Context.ConnectionId);
            }

        }).Select(x => x.Key);
    }

    protected SignalRUser GetUser(string username)
    {
        SignalRUser user;
        _users.TryGetValue(username, out user);
        return user;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _sl != null)
            _sl.Dispose();

        base.Dispose(disposing);
    }

}
