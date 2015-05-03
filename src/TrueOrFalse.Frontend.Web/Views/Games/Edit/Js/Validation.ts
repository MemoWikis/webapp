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
}

$(() => {
    var validator = fnValidateForm("#EditGameForm", validationSettings_Game, false);
});