using System;

public class BaseModel : BaseResolve
{
    public MenuLeftModel MenuLeftModel = new MenuLeftModel();

    protected SessionUser _sessionUser { get { return Resolve<SessionUser>(); } }
    protected SessionUiData _sessionUiData { get { return Resolve<SessionUiData>(); } }

    public bool IsLoggedIn{ get { return _sessionUser.IsLoggedIn; } }

    public int UserId{ get { return _sessionUser.UserId; } }

    public Game UpcomingGame;
    
    public bool IsInGame;

    public bool IsCreatorOfGame;

    public BaseModel()
    {
        if (IsLoggedIn)
        {
            var isInGame = R<IsCurrentlyIn>().Game(UserId);
            if (isInGame.Yes)
            {
                UpcomingGame = isInGame.Game;
                IsInGame = true;
                IsCreatorOfGame = isInGame.IsCreator;
            }
            else
                UpcomingGame = new Game();
        }


    }
}