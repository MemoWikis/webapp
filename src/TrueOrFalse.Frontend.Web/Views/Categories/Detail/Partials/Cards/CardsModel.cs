using System.Collections.Generic;

public class CardsModel : BaseContentModule
{
    public IList<Set> Sets;
    public string Title;
    public string CardOrientation;

    public CardsModel(CardsJson cardsjson)
    {
        Sets = cardsjson.GetSetList();
        Title = cardsjson.TemplateName;

        if (cardsjson.CardOrientation == "Landscape" || cardsjson.CardOrientation == "Portrait")
            CardOrientation = cardsjson.CardOrientation;
        else
        {
            CardOrientation = Sets.Count % 3 == 0 ? "Portrait" : "Landscape";
        }
    }
}