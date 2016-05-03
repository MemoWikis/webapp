class TourInit {
    constructor() {
        console.log("TourInit.ts-constructor started");
        $("#btnStartWelcomeTour").click(function () {
            // Initialize the tour
            tourWelcome.init();

            // Start the tour
            tourWelcome.start();
        });
        
    }
}