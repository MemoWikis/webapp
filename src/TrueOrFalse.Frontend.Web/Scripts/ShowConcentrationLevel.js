var ShowConcentrationLevel = (function () {
    function ShowConcentrationLevel() {
        var hub = $.connection.brainWavesHub;

        window.console.log(hub);

        if (hub == null)
            return;

        hub.client.UpdateConcentrationLevel = function (level) {
            window.alert(level);
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    return ShowConcentrationLevel;
})();

$(function () {
    new ShowConcentrationLevel();
});
//# sourceMappingURL=ShowConcentrationLevel.js.map
