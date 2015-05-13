class ToolsBrainWaveHub {
    constructor() {

        var hub = $.connection.brainWavesHub;
        if (hub == null) {
            window.alert("hub is null");
            return;
        }

        hub.client.UserConnected = (userId: string) => {
            this.AddConnectedUser(userId);
        };

        $.connection.hub.start(() => {
            hub.server.getConnectedUsers().done(users => {
                $.each(users, (i, userId) => {
                    this.AddConnectedUser(userId);
                });
            });
        });

        $("#btnSendBrainWaveValue").click((e) => {
            e.preventDefault();

            var concentrationLevel = $("#TxtConcentrationLevel").val();
            var userId = $("#TxtUserId").val();

            hub.server.sendConcentration(concentrationLevel, userId).done(() => {
                this.ShowFeedback("Success: Message send");
            }).fail(error => {
                this.ShowFeedback(error);
            });
        });
    }

    ShowFeedback(msg: string) {
        Utils.SetElementValue2($("#msgConcentrationLevel"), msg);
    }

    AddConnectedUser(userId: string) {
        $("#connectedUsers").append($("<span style='padding-right: 10px;'>" + userId + "</span>"));
    }
 }

 $(() => {
     new ToolsBrainWaveHub();
 });