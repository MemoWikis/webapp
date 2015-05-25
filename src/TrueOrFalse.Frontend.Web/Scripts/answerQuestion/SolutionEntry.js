var SolutionEntry = (function () {
    function SolutionEntry() {
        var solutionT = $("hddSolutionType").val();

        switch (solutionT) {
            case 6 /* Date */:
                new SolutionTypeDateEntry();
                break;
            case 3 /* MultipleChoice */:
                new SolutionTypeMultipleChoice();
                break;
            case 4 /* Numeric */:
                new SolutionTypeTextEntry();
                break;
            case 5 /* Sequence */:
                new SolutionTypeNumeric();
                break;
            case 1 /* Text */:
                new SolutionTypeSequence();
                break;
        }
        ;
    }
    return SolutionEntry;
})();

$(function () {
    new SolutionEntry();
});
//# sourceMappingURL=SolutionEntry.js.map
