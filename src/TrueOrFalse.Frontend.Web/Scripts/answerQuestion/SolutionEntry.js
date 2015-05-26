var SolutionEntry = (function () {
    function SolutionEntry() {
    }
    SolutionEntry.prototype.Init = function () {
        var solutionT = +$("#hddSolutionTypeNum").val();

        switch (solutionT) {
            case 6 /* Date */:
                new SolutionTypeDateEntry();
                break;
            case 3 /* MultipleChoice */:
                new SolutionTypeMultipleChoice();
                break;
            case 1 /* Text */:
                new SolutionTypeTextEntry();
                break;
            case 4 /* Numeric */:
                new SolutionTypeNumeric();
                break;
            case 5 /* Sequence */:
                new SolutionTypeSequence();
                break;
        }
        ;
    };
    return SolutionEntry;
})();
//# sourceMappingURL=SolutionEntry.js.map
