using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class GameModel : BaseModel
{
    public int Id;
    public UIMessage Message;

    [Required][Range(2, 30)]
    public virtual int MaxPlayers { get; set; }
    
    [Required][Range(1, 100)]
    public virtual int Rounds { get; set; }

    [Required][Range(1, 60)]
    public virtual int StartsInMinutes { get; set; }
    
    [Required]
    public virtual string Sets { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Bemerkung:")]
    public string Comment { get; set; }

    public GameModel() { }

}