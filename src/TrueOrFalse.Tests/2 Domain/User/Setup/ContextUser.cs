using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

public class ContextUser
{
    private readonly UserRepo _userRepo;

    public List<User> All = new();

    private ContextUser(UserRepo userRepo)
    {
        _userRepo = userRepo;
    }

    public static ContextUser New(UserRepo userRepo)
    {
        return new ContextUser(userRepo);
    }

    public static User GetUser(UserRepo userRepo, string userName = "Firstname Lastname")
    {
        return New(userRepo).Add(userName).Persist().All[0];
    }

    public ContextUser Add(string userName)
    {
        All.Add(new User { Name = userName });
        return this;
    }

    public ContextUser AddWithEmail(string mailAddress)
    {
        All.Add(new User { EmailAddress = mailAddress });
        return this;
    }

    public ContextUser Add() 
    {
        All.Add(new User {
            Id = 0, 
            Name = "Daniel" });
        return this;
    }

    public ContextUser Add(User user)
    {
        All.Add(user);
        return this;
    }

    public ContextUser Persist(bool withStartTopic = false, ContextCategory context = null)
    {
        foreach (var usr in All)
        {
            _userRepo.Create(usr);
            if (withStartTopic && usr != null)
            {
                Category firstStartTopic;
                if (context != null)
                {
                    var newId = context.All.Count + 1;
                    firstStartTopic = context
                        .Add(usr.Name + "s Startseite", creator: usr, id: newId).Persist()
                        .All
                        .ByName(usr.Name + "s Startseite");
                }
                else
                    firstStartTopic = ContextCategory.New(false) 
                    .Add(usr.Name + "s Startseite", creator: usr).Persist()
                    .All
                    .First();
                usr.StartTopicId = firstStartTopic.Id;
            }
        }
        return this;
    }
}
