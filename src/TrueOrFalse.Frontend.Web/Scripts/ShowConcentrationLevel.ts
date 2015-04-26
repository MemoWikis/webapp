class ShowConcentrationLevel {
    constructor() {

        var hub = $.connection.brainWavesHub;

        window.console.log(hub);

        if (hub == null)
            return;

        hub.client.UpdateConcentrationLevel = (level : string) => {
            window.alert(level);
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });
    }
}

$(() => {
    new ShowConcentrationLevel();
});