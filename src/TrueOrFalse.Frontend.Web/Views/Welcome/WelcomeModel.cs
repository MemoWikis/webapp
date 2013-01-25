using System.Collections.Generic;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Models;


public class WelcomeModel : BaseModel
{
    public IList<Question> MostPopular = new List<Question>();
    public IList<Question> MostWantend = new List<Question>();
    public IList<Question> MostImportant = new List<Question>();

    public string Date;
    
    public WelcomeModel()
    {
        MostPopular.Add(new Question{Text = "\"Wenn ich über die steuer- und erbrechtliche Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren.\" Wer hat das gesagt?"});
        MostPopular.Add(new Question { Text = "Wieviele Mitarbeiter arbeiten bei der Deutschen Bahn?" });
        MostPopular.Add(new Question { Text = "24" });

        MostWantend.Add(new Question());
    }
}
