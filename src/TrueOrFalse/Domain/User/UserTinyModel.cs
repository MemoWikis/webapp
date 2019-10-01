using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seedworks.Lib.Persistence;


public interface IUserTinyModel
{
    string Name { get; set; }
    int Id { get; set; }
    string EmailAddress { get; set; }
    bool IsFacebookUser { get; }
    bool IsGoogleUser { get; }
    string FacebookId { get; set; }

    string GoogleId { get; set; }
}

public class UserTinyModel : IUserTinyModel
{
    public string Name { get; set; }
    public int Id { get; set; }
    public string EmailAddress { get; set; }
    public bool IsFacebookUser { get; set; }
    public bool IsGoogleUser { get; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }

    private readonly User _user;

    public UserTinyModel(User user)
    {
        _user = user;

        if (_user == null)
        {
            Name = "Unbekannt";
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
    }
}
