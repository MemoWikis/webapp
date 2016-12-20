class TourInit {

    currentStep: number;

    constructor() {
        $("#btnStartWelcomeTour").click(function () {
            // Initialize the tour
            tourWelcome.init();
            //tourWelcome.setCurrentStep(0); //would start the tour at step 0 every time the button is clicked
            // Start the tour
            tourWelcome.start(true); //true forces the start
        });
        
    }
}