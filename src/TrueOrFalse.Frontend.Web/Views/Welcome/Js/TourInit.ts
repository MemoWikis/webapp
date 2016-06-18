class TourInit {

    currentStep: number;

    constructor() {
        //console.log("TourInit.ts-constructor started");
        $("#btnStartWelcomeTour").click(function () {
            //console.log("TourInit; currentStep: " + tourWelcome.getCurrentStep());
            //window.location.href = window.hostname;
            // Initialize the tour
            tourWelcome.init();

            // Start the tour
            tourWelcome.start(true); //true forces the start
        });
        
    }
}