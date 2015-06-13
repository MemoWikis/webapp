using System;

public class BaseSendMessage
{
    protected readonly MessageRepository _messageRepo;
    protected readonly UserRepo _userRepo;

    public BaseSendMessage(MessageRepository messageRepo, UserRepo userRepo)
    {
        _messageRepo = messageRepo;
        _userRepo = userRepo;
    }

    protected User LoadUser(int receiverId)
    {
        var user = _userRepo.GetById(receiverId);

        if (user == null)
            throw new Exception("user '" + receiverId + "' not found");

        return user;
    }
}