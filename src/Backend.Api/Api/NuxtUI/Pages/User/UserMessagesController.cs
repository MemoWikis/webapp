using System.Globalization;

public class UserMessagesController(
    SessionUser _sessionUser,
    MessageRepo _messageRepo) : ApiBaseController
{
    public readonly record struct GetResult(Message[] Messages, int ReadCount, bool? NotLoggedIn);

    public readonly record struct Message(
        int Id,
        bool Read,
        string Subject,
        string Body,
        DateTime DateCreated,
        string Date);

    [HttpGet]
    public GetResult Get()
    {
        if (_sessionUser.IsLoggedIn)
        {
            var messages = _messageRepo
                .GetForUser(_sessionUser.UserId)
                .Select(m => new Message
                {
                    Id = m.Id,
                    Read = m.IsRead,
                    Subject = m.Subject,
                    Body = m.Body,
                    DateCreated = m.DateCreated,
                    Date = m.DateCreated.ToString("", new CultureInfo("de-DE"))
                })
                .ToArray();

            var readMessagesCount = _messageRepo.GetNumberOfReadMessages(_sessionUser.UserId);
            return new GetResult
            {
                Messages = messages,
                ReadCount = readMessagesCount
            };
        }

        return new GetResult
        {
            NotLoggedIn = true,
        };
    }
}