var validationSettings_Game = {
    rules: {
        StartsInMinutes: {
            required: true
        },
        MaxPlayers: {
            required: true
        },
        Sets: {
            required: true
        }
    }
};

$(function () {
    var validator = fnValidateForm("#EditGameForm", validationSettings_Game, false);
});
//# sourceMappingURL=Validation.js.map
