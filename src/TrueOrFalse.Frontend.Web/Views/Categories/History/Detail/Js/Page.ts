declare var difflib;
declare var Diff2Html;
declare var Diff2HtmlUI;

$(() => {

    ShowDiff2Html();

});

function ShowDiff2Html() {

    

    var currentRevData = $('#currentRevData').val();
    var previousRevData = $('#previousRevData').val();
    var currentRevDescription = $('#currentRevDescription').val();
    var previousRevDescription= $('#previousRevDescription').val();

    var diffData = Diff(previousRevData, currentRevData, 'Änderungen des Inhaltes');
    var diffDescription = Diff(previousRevDescription, currentRevDescription, 'Änderungen der Beschreibung');
    
    if (diffDescription)
        ShowDiff(diffDescription, '#diffDescription');
    if (diffData)
        ShowDiff(diffData, '#diffData');
    if (!diffData && !diffDescription) {
        $("#diffPanel").hide();
        $("#nochangesdiv").show();
    }
}

function Diff(prev, current, name) {
    return difflib.unifiedDiff(
        prev.split('\n'),
        current.split('\n'),
        {
            fromfile: name,
            tofile: name,
            lineterm: ''
        }).join("\n");
}

function ShowDiff(data, divId) {
    var diff2htmlUi = new Diff2HtmlUI({ diff: data });
    diff2htmlUi.draw(divId,
        {
            inputFormat: 'diff',
            outputFormat: 'line-by-line',
            showFiles: false,
            matching: 'none'
        });
}