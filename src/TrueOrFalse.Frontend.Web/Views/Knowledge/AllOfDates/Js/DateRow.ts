 class DateRow {
     
     static HideRow(dateId : number) {
         $("[data-date-id="+ dateId +"]").fadeOut(600);
     }
 }