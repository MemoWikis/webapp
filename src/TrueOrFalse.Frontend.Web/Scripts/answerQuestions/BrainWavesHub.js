var BrainWavesHub = (function () {
    function BrainWavesHub() {
        var hub = $.connection.brainWavesHub;

        if (hub == null)
            return;

        hub.client.UpdateConcentrationLevel = function (level) {
            $("#concentrationLevel").html(level);
        };

        hub.client.UpdateMellowLevel = function (level) {
            $("#mellowLevel").html(level);
        };

        hub.client.DisconnectEEG = function () {
            $("#concentrationLevel").html("");
            $("#mellowLevel").html("disconnected");
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    return BrainWavesHub;
})();

$(function () {
    new BrainWavesHub();
});
//# sourceMappingURL=BrainWavesHub.js.map
