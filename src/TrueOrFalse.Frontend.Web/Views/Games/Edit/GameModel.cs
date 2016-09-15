using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class GameModel : BaseModel
{
    public int Id;
    public UIMessage Message;

    //Validation serves as backup for client side validation

    [Required][Range(2, 30)]
    public int MaxPlayers { get; set; }
    
    [Required][Range(1, 100)]
    public int Rounds { get; set; }

    [Required][Range(1, 60)]
    public int StartsInMinutes { get; set; }
    
    public IList<Set> Sets = new List<Set>();

    [DataType(DataType.MultilineText)]
    [DisplayName("Bemerkung:")]
    public string Comment { get; set; }

    public bool OnlyMultipleChoice { get; set; }

    public bool WithSystemAvgPlayer { get; set; }

    public GameModel()
    {
        OnlyMultipleChoice = true;
        WithSystemAvgPlayer = false;
    }

}