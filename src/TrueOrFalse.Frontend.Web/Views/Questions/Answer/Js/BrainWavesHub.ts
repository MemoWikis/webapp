class BrainWavesHub {
    constructor() {

        var hub = $.connection.brainWavesHub;

        if (hub == null)
            return;

        hub.client.UpdateConcentrationLevel = (level: string) => {
            $("#concentrationLevel").html(level);
        };

        hub.client.UpdateMellowLevel = (level: string) => {
            $("#mellowLevel").html(level);
        };

        hub.client.DisconnectEEG = () => {
            $("#concentrationLevel").html("");
            $("#mellowLevel").html("disconnected");
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });

    }
}

$(() => {
    new BrainWavesHub();
});