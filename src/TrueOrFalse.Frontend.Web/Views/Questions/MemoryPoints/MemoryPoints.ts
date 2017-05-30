class MemoryPoints {
    constructor() {
    }

    public updatePointsDisplay() {
        $("#memoryPointsDispaly #memoryPoints").html(/*write Points from session in here*/);
    }

    public addPoitsFromRightAnswer() {
        this.addPoints(15);
    }

    public addPoitsFromWrongAnswer() {
        this.addPoints(1);
    }

    public addPoitsFromSolutionViewAnswer() {
        this.addPoints(3);
    }


    private addPoints(amount: number) {
        //add points to session here
    }

}