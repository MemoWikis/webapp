declare var difflib;
declare var Diff2Html;
declare var Diff2HtmlUI;

$(() => {

    ShowDiff2Html();

});

function ShowDiff2Html() {

    var currentRevData = $('#currentRevData').val();
    var previousRevData = $('#previousRevData').val();
    var currentRevDateCreated = $('#currentRevDateCreated').val();
    var previousRevDateCreated = $('#previousRevDateCreated').val();
    
    var difflibRes = difflib.unifiedDiff(
        previousRevData.split('\n'),
        currentRevData.split('\n'),
        {
            fromfile: 'Revision',
            tofile: 'Revision',
            fromfiledate: previousRevDateCreated,
            tofiledate: currentRevDateCreated,
            lineterm: ''
        }).join("\n");

    //var diff = JsDiff.diffChars(one, other),
    //    display = document.getElementById('display'),
    //    fragment = document.createDocumentFragment();
    
    if (difflibRes) {
        var diff2htmlUi = new Diff2HtmlUI({ diff: difflibRes });
        diff2htmlUi.draw('#outputdiv',
            {
                inputFormat: 'diff',
                outputFormat: 'line-by-line',
                showFiles: false,
                matching: 'none'
            });
    } else {
        $("#outputdiv").hide();
        $("#nochangesdiv").show();
    }
}