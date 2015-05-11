var validationSettings_Game = {
    rules: {
        StartsInMinutes: {
            required: true,
            range: [2, 60]
        },
        MaxPlayers: {
            required: true,
            range: [2, 30]
        },
        Sets: {
            required: true
        },
        Rounds: {
            required: true,
            range: [1, 100]
        }
    }
};

$(function () {
    var validator = fnValidateForm("#EditGameForm", validationSettings_Game, false);
});
//# sourceMappingURL=Validation.js.map
