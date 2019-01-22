declare var difflib;
declare var Diff2Html;
declare var Diff2HtmlUI;

$(() => {
    
    ShowQuestionDiff2Html();

});

function ShowQuestionDiff2Html() {
    
    var currentQuestionText = $('#currentQuestionText').val();
    var prevQuestionText = $('#prevQuestionText').val();
    var diffQuestionText = QuestionDiff(prevQuestionText, currentQuestionText, 'Änderungen der Frage');

    var currentQuestionTextExtended = $('#currentQuestionTextExtended').val();
    var prevQuestionTextExtended = $('#prevQuestionTextExtended').val();
    var diffQuestionTextExtended = QuestionDiff(prevQuestionTextExtended, currentQuestionTextExtended, 'Änderungen der erweiterten Fragedefinition (u.a. Bildinformationen)');

    var currentLicense = $('#currentLicense').val();
    var prevLicense = $('#prevLicense').val();
    var diffLicense = QuestionDiff(prevLicense, currentLicense, 'Änderungen der Lizenzinformationen');

    var currentVisibility = $('#currentVisibility').val();
    var prevVisibility = $('#prevVisibility').val();
    var diffVisibility = QuestionDiff(prevVisibility, currentVisibility, 'Änderungen der Sichtbarkeit');

    var currentSolution = $('#currentSolution').val();
    var prevSolution = $('#prevSolution').val();
    var diffSolution = QuestionDiff(prevSolution, currentSolution, 'Änderungen der Lösung');

    var currentSolutionDescription = $('#currentSolutionDescription').val();
    var prevSolutionDescription = $('#prevSolutionDescription').val();
    var diffSolutionDescription = QuestionDiff(prevSolutionDescription, currentSolutionDescription, 'Änderungen der Lösungsbeschreibung');

    var currentSolutionMetadataJson = $('#currentSolutionMetadataJson').val();
    var prevSolutionMetadataJson = $('#prevSolutionMetadataJson').val();
    var diffSolutionMetadataJson = QuestionDiff(prevSolutionMetadataJson, currentSolutionMetadataJson, 'Änderungen der Metadaten der Lösung');

    var imageWasChanged = $('#imageWasChanged').val().toLowerCase();
    
    if (diffQuestionText)
        ShowQuestionDiff(diffQuestionText, '#diffQuestionText');
    if (diffQuestionTextExtended)
        ShowQuestionDiff(diffQuestionTextExtended, '#diffQuestionTextExtended');
    if (diffLicense)
        ShowQuestionDiff(diffLicense, '#diffLicense');
    if (diffVisibility)
        ShowQuestionDiff(diffVisibility, '#diffVisibility');
    if (diffSolution)
        ShowQuestionDiff(diffSolution, '#diffSolution');
    if (diffSolutionDescription)
        ShowQuestionDiff(diffSolutionDescription, '#diffSolutionDescription');
    if (diffSolutionMetadataJson)
        ShowQuestionDiff(diffSolutionMetadataJson, '#diffSolutionMetadataJson');
    
    // Zeige Hinweis, falls es keine inhaltichen Änderungen zu geben scheint
    if (!diffQuestionText && !diffQuestionTextExtended && !diffLicense && !diffVisibility && !diffSolution && !diffSolutionDescription && !diffSolutionMetadataJson && imageWasChanged === "false")
    {
        $("#diffPanel").hide();
        $("#noChangesAlert").show();
    }
}

function QuestionDiff(prev, current, name) {
    return difflib.unifiedDiff(
        prev.split('\n'),
        current.split('\n'),
        {
            fromfile: name,
            tofile: name,
            lineterm: ''
        }).join("\n");
}

function ShowQuestionDiff(data, divId) {
    var diff2htmlUi = new Diff2HtmlUI({ diff: data });
    diff2htmlUi.draw(divId,
        {
            inputFormat: 'diff',
            outputFormat: 'line-by-line',
            showFiles: false,
            matching: 'none'
        });
}