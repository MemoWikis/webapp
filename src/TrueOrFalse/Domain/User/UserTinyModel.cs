public interface IUserTinyModel
{
    int Id { get; set;}
    string Name { get; }
    string EmailAddress { get;}
    bool IsFacebookUser { get;}
    bool IsGoogleUser { get;}           
    string FacebookId { get; }
    int Reputation { get; }
    string GoogleId { get;}
    bool ShowWishKnowledge { get; }
    int FollowerCount { get; }
}

public class UserTinyModel : IUserTinyModel
{
    public int Id { get; set; }
    public string Name { get;  }
    public string EmailAddress { get; }
    public bool IsFacebookUser { get;  }
    public bool IsGoogleUser { get; }
    public string FacebookId { get;  }
    public string GoogleId { get;  }
    public int Reputation { get; }
    public int ReputationPos { get; }
    public bool ShowWishKnowledge { get; set; }
    public bool IsMember { get; }

    private readonly User _user;

    public User User => _user;

    public bool IsMemuchoUser { get; }

    public bool IsKnown => !IsUnknown;
    public bool IsUnknown { get; }
    public int FollowerCount { get;  }

    public UserTinyModel(User user)
    {
        _user = user;

        if (_user == null)
        {
            Name = "Unbekannt";
            IsUnknown = true;
        }
        else
        {
            Name = _user.Name;
            EmailAddress = _user.EmailAddress;
        }

        EmailAddress = _user == null ? "unbekannt" : _user.EmailAddress;
        IsFacebookUser = _user != null && _user.IsFacebookUser;
        IsGoogleUser = _user != null && _user.IsGoogleUser;
        Id = _user == null ? -1 : _user.Id;
        FacebookId = _user == null ? "null" : _user.FacebookId;
        GoogleId = _user == null ? "null" : _user.GoogleId;
        IsMemuchoUser = _user != null && _user.IsMemuchoUser;
        Reputation = _user != null ? _user.Reputation :  0;
        ReputationPos = _user != null ? _user.ReputationPos : 0;
        IsMember = _user != null && _user.IsMember();
        FollowerCount = _user != null ? _user.FollowerCount : 0;
    }
}
