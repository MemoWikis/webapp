$(function () {
    new DateRowDelete();
    new PreviousDates();

    var trainingSettings = new TrainingSettings();
    trainingSettings.Populate(24);
});