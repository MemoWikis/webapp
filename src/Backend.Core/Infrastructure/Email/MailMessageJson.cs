class MailMessageJson
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }


    public MailMessageJson(string from, string to, string subject, string body)
    {
        this.From = from;
        this.To = to;
        this.Subject = subject;
        this.Body = body;
    }
}