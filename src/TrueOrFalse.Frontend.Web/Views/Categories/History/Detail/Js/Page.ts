declare var difflib;
declare var Diff2Html;
declare var Diff2HtmlUI;

$(() => {

    ShowDiff2Html();

});

function ShowDiff2Html() {

    var currentData = $('#currentData').val();
    var previousData = $('#previousData').val();

    var difflibRes = difflib.unifiedDiff(
        currentData.split('\n'),
        previousData.split('\n'),
        {
            fromfile: 'Original',
            tofile: 'Current',
            fromfiledate: '2005-01-26 23:30:50',
            tofiledate: '2010-04-02 10:20:52',
            lineterm: ''
        }).join("\r\n");

    var diff2htmlUi = new Diff2HtmlUI({ diff: difflibRes });
    diff2htmlUi.draw('#outputdiv',
        {
            inputFormat: 'diff',
            outputFormat: 'line-by-line',
            showFiles: false,
            matching: 'none'
        });
}