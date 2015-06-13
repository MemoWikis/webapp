var ToolsBrainWaveHub = (function () {
    function ToolsBrainWaveHub() {
        var _this = this;
        var hub = $.connection.brainWavesHub;
        if (hub == null) {
            window.alert("hub is null");
            return;
        }

        hub.client.UserConnected = function (userId) {
            _this.AddConnectedUser(userId);
        };

        $.connection.hub.start(function () {
            hub.server.getConnectedUsers().done(function (users) {
                $.each(users, function (i, userId) {
                    _this.AddConnectedUser(userId);
                });
            });
        });

        $("#btnSendBrainWaveValue").click(function (e) {
            e.preventDefault();

            var concentrationLevel = $("#TxtConcentrationLevel").val();
            var userId = $("#TxtUserId").val();

            hub.server.sendConcentration(concentrationLevel, userId).done(function () {
                _this.ShowFeedback("Success: Message send");
            }).fail(function (error) {
                _this.ShowFeedback(error);
            });
        });
    }
    ToolsBrainWaveHub.prototype.ShowFeedback = function (msg) {
        Utils.SetElementValue2($("#msgConcentrationLevel"), msg);
    };

    ToolsBrainWaveHub.prototype.AddConnectedUser = function (userId) {
        $("#connectedUsers").append($("<span style='padding-right: 10px;'>" + userId + "</span>"));
    };
    return ToolsBrainWaveHub;
})();

$(function () {
    new ToolsBrainWaveHub();
});
//# sourceMappingURL=ToolsBrainWaveHub.js.map
