using Microsoft.AspNet.SignalR;

public class BrainWavesHub : Hub
{
    public void Send(int concentrationLevel)
    {
        Clients.All.UpdateConcentrationLevel(concentrationLevel);
    }
}