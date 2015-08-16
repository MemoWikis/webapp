class GameHub {
    
    private static _hub: any;
    private static _isInitialized = false;

    private static _onStartTimeChangedFns: { (willStartAt: ChangeStartTimeEvent): void }[] = [];

    private static Init(){

        if (this._isInitialized){
            return;
        }

        this._isInitialized = true;

        this._hub = $.connection.gameHub;

        this._hub.client.ChangeStartTime = (changeStartTime) => {
            for (var i = 0; i < this._onStartTimeChangedFns.length; i++) {
                this._onStartTimeChangedFns[i](changeStartTime);
            }
        };

        if ($.connection.hub.state === $.signalR.connectionState.disconnected) {
            $.connection.hub.start(() => {
                window.console.log("game hub connection started:");
            });
        }
    }

    public static OnChangeStartTime(fn: (willStartAt: ChangeStartTimeEvent) => void): GameHub {

        GameHub.Init();

        this._onStartTimeChangedFns.push(fn);
        return this;
    }

}