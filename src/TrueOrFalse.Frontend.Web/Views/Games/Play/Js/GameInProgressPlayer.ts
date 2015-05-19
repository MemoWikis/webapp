class GameInProgressPlayer {

    _play : Play;

    constructor(play : Play) {
        this._play = play;

        this._play.Hub.client.NextRound = (game: Game) => {
            Utils.SetElementValue("#CurrentRoundNum", game.Round.toString());
        };
    }
}