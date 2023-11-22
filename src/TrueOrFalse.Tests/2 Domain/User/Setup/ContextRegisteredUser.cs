using System;
using System.Collections.Generic;
using BDDish.Model;

public class ContextRegisteredUser : IContextDescription
{
    private readonly UserReadingRepo _userReadingRepo;
    private readonly UserWritingRepo _userWritingRepo;

    public string SampleDesciption { get; set; }
    public string EmailAddress = "john@doe.com";

    public List<User> Users = new List<User>();

    private ContextRegisteredUser(UserReadingRepo userReadingRepo,
        UserWritingRepo userWritingRepo)
    {
        _userReadingRepo = userReadingRepo;
        _userWritingRepo = userWritingRepo;
    }

    public static ContextRegisteredUser New(UserReadingRepo userReadingRepo, UserWritingRepo userWritingRepo)
    {
        return new ContextRegisteredUser(userReadingRepo, userWritingRepo);
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
            _userWritingRepo.Create(user);

        return this;
    }

    public void Setup() { Add(); }

    public ContextRegisteredUser SetEmailAddress(string emailAddress) { EmailAddress = emailAddress; return this; }
}