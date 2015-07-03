﻿var validationSettings_Game = {
    rules: {
        StartsInMinutes: {
            required: true,
            range: [1, 10]
        },
        MaxPlayers: {
            required: true,
            range: [2, 30]
        },
        Rounds: {
            required: true,
            range: [1, 100]
        }
    }
};
//# sourceMappingURL=Validation.js.map
