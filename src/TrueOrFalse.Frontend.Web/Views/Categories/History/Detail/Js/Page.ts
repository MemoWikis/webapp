declare var difflib;
declare var Diff2Html;
declare var Diff2HtmlUI;

$(() => {

    ShowDiff2Html();

});

function ShowDiff2Html() {

    var currentMarkdown = $('#currentMarkdown').val();
    var prevMarkdown = $('#prevMarkdown').val();
    var currentDescription = $('#currentDescription').val();
    var prevDescription = $('#prevDescription').val();
    var currentWikipediaUrl = $('#currentWikipediaUrl').val();
    var prevWikipediaUrl = $('#prevWikipediaUrl').val();

    var diffData = Diff(prevMarkdown, currentMarkdown, 'Änderungen des Inhaltes');
    var diffDescription = Diff(prevDescription, currentDescription, 'Änderungen der Beschreibung');
    var diffWikipediaUrl = Diff(prevWikipediaUrl, currentWikipediaUrl, 'Änderungen der Wikipedia-URL');

    if (diffDescription)
        ShowDiff(diffDescription, '#diffDescription');
    if (diffWikipediaUrl)
        ShowDiff(diffWikipediaUrl, '#diffWikipediaUrl');
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