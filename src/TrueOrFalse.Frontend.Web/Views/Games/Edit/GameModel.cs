using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class GameModel : BaseModel
{
    public int Id;
    public UIMessage Message;

    [Required]
    public virtual int MaxPlayers { get; set; }
    [Required]
    public virtual int StartsInMinutes { get; set; }
    [Required]
    public virtual string Sets { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Bemerkung:")]
    public string Comment { get; set; }

    public GameModel() { }

}