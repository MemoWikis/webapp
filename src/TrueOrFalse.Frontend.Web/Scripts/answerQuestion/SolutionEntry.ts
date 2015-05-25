 class SolutionEntry {
     constructor() {

         var solutionT = <SolutionType>$("hddSolutionType").val();

         switch (solutionT) {
            case SolutionType.Date:
                new SolutionTypeDateEntry(); break;
            case SolutionType.MultipleChoice:
                new SolutionTypeMultipleChoice(); break;
            case SolutionType.Numeric:
                new SolutionTypeTextEntry(); break;
            case SolutionType.Sequence:
                new SolutionTypeNumeric(); break;
            case SolutionType.Text:
                new SolutionTypeSequence(); break;
         };
     }
 }

 $(() => {
     new SolutionEntry();
 });