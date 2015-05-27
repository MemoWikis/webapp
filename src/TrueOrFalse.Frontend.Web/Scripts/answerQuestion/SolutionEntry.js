var SolutionEntry = (function () {
    function SolutionEntry(isGameMode) {
        if (typeof isGameMode === "undefined") { isGameMode = false; }
        this.IsGameMode = isGameMode;
    }
    SolutionEntry.prototype.Init = function () {
        var solutionT = +$("#hddSolutionTypeNum").val();

        switch (solutionT) {
            case 6 /* Date */:
                new SolutionTypeDateEntry(this);
                break;
            case 3 /* MultipleChoice */:
                new SolutionTypeMultipleChoice(this);
                break;
            case 1 /* Text */:
                new SolutionTypeTextEntry(this);
                break;
            case 4 /* Numeric */:
                new SolutionTypeNumeric(this);
                break;
            case 5 /* Sequence */:
                new SolutionTypeSequence(this);
                break;
        }
        ;
    };
    return SolutionEntry;
})();
//# sourceMappingURL=SolutionEntry.js.map
