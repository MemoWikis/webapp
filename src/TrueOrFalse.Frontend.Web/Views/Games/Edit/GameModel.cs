using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrueOrFalse.Web;

public class GameModel : BaseModel
{
    public int Id;
    public bool IsEditing;
    public UIMessage Message;

    public IList<Set> Sets;
    public GameStatus Status;
    public DateTime WillStartAt;


    [DataType(DataType.MultilineText)]
    [DisplayName("Bemerkung:")]
    public string Comment { get; set; }

    public GameModel() { }

    public GameModel(Game game)
    {
        IsEditing = true;
        Id = game.Id;

        Sets = game.Sets;
        Status = game.Status;
        WillStartAt = game.WillStartAt;
        Comment = game.Comment;
    }

    public Game ToGame()
    {
        var game = new Game();
        game.WillStartAt = DateTime.Now.AddMinutes(10);
        game.Comment = Comment;
        game.Id = game.Id;

        return game;
    }
}