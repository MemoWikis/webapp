class BrainWavesHub {
    constructor() {

        var hub = $.connection.brainWavesHub;

        if (hub == null)
            return;

        hub.client.UpdateConcentrationLevel = (level: string) => {
            $("#conentrationLevel").html(level);
        };

        $.connection.hub.start(() => {
            window.console.log("connection started:");
        });

    }
}

$(() => {
    new BrainWavesHub();
});