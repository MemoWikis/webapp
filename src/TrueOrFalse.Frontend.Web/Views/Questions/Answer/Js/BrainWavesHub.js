var BrainWavesHub = (function () {
    function BrainWavesHub() {
        var hub = $.connection.brainWavesHub;

        if (hub == null)
            return;

        hub.client.UpdateConcentrationLevel = function (level) {
            $("#conentrationLevel").html(level);
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
