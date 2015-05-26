 class SolutionEntry {

     public Init() {

         var solutionT = +$("#hddSolutionTypeNum").val();

         switch (solutionT) {
             case SolutionType.Date:
                 new SolutionTypeDateEntry(); break;
             case SolutionType.MultipleChoice:
                 new SolutionTypeMultipleChoice(); break;
             case SolutionType.Text:
                 new SolutionTypeTextEntry(); break;
             case SolutionType.Numeric:
                 new SolutionTypeNumeric(); break;
             case SolutionType.Sequence:
                 new SolutionTypeSequence(); break;
         };         
     }
 }