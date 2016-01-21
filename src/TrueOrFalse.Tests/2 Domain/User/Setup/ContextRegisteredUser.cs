using System;
using System.Collections.Generic;
using BDDish.Model;

public class ContextRegisteredUser : IContextDescription
{
    private readonly UserRepo _userRepo;

    public string SampleDesciption { get; set; }
    public string EmailAddress = "john@doe.com";

    public List<User> Users = new List<User>();

    private ContextRegisteredUser()
    {
        _userRepo = Sl.R<UserRepo>();
    }

    public static ContextRegisteredUser New()
    {
        return new ContextRegisteredUser();
    }

    public ContextRegisteredUser Add()
    {
        var user = new User();
        user.EmailAddress = EmailAddress;
        user.Birthday = new DateTime(1980, 08, 03);
        Users.Add(user);

        return this;
    }

    public ContextRegisteredUser Persist()
    {
        foreach(var user in Users)
            _userRepo.Create(user);

        return this;
    }

    public void Setup() { Add(); }

    public ContextRegisteredUser SetEmailAddress(string emailAddress) { EmailAddress = emailAddress; return this; }
}