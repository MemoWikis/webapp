declare var difflib;
declare var Diff2Html;
declare var Diff2HtmlUI;

function ShowQuestionDiff2Html(revId) {
    
    var currentQuestionText = $('#currentQuestionText_' + revId).val();
    var prevQuestionText = $('#prevQuestionText_' + revId).val();
    var diffQuestionText = QuestionDiff(prevQuestionText, currentQuestionText, 'Änderungen der Frage');
    
    var currentQuestionTextExtended = $('#currentQuestionTextExtended_' + revId).val();
    var prevQuestionTextExtended = $('#prevQuestionTextExtended_' + revId).val();
    var diffQuestionTextExtended = QuestionDiff(prevQuestionTextExtended, currentQuestionTextExtended, 'Änderungen der erweiterten Fragedefinition (u.a. Bildinformationen)');

    var currentLicense = $('#currentLicense_' + revId).val();
    var prevLicense = $('#prevLicense_' + revId).val();
    var diffLicense = QuestionDiff(prevLicense, currentLicense, 'Änderungen der Lizenzinformationen');

    var currentVisibility = $('#currentVisibility_' + revId).val();
    var prevVisibility = $('#prevVisibility_' + revId).val();
    var diffVisibility = QuestionDiff(prevVisibility, currentVisibility, 'Änderungen der Sichtbarkeit');

    var currentSolution = $('#currentSolution_' + revId).val();
    var prevSolution = $('#prevSolution_' + revId).val();
    var diffSolution = QuestionDiff(prevSolution, currentSolution, 'Änderungen der Lösung');

    var currentSolutionDescription = $('#currentSolutionDescription_' + revId).val();
    var prevSolutionDescription = $('#prevSolutionDescription_' + revId).val();
    var diffSolutionDescription = QuestionDiff(prevSolutionDescription, currentSolutionDescription, 'Änderungen der Lösungsbeschreibung');

    var currentSolutionMetadataJson = $('#currentSolutionMetadataJson_' + revId).val();
    var prevSolutionMetadataJson = $('#prevSolutionMetadataJson_' + revId).val();
    var diffSolutionMetadataJson = QuestionDiff(prevSolutionMetadataJson, currentSolutionMetadataJson, 'Änderungen der Metadaten der Lösung');

    var imageWasChanged = $('#imageWasChanged_' + revId).val().toLowerCase();
    
    if (diffQuestionText)
        ShowQuestionDiff(diffQuestionText, '#diffQuestionText_' + revId);
    if (diffQuestionTextExtended)
        ShowQuestionDiff(diffQuestionTextExtended, '#diffQuestionTextExtended_' + revId);
    if (diffLicense)
        ShowQuestionDiff(diffLicense, '#diffLicense_' + revId);
    if (diffVisibility)
        ShowQuestionDiff(diffVisibility, '#diffVisibility_' + revId);
    if (diffSolution)
        ShowQuestionDiff(diffSolution, '#diffSolution_' + revId);
    if (diffSolutionDescription)
        ShowQuestionDiff(diffSolutionDescription, '#diffSolutionDescription_' + revId);
    if (diffSolutionMetadataJson)
        ShowQuestionDiff(diffSolutionMetadataJson, '#diffSolutionMetadataJson_' + revId);
    
    // Zeige Hinweis, falls es keine inhaltichen Änderungen zu geben scheint
    if (!diffQuestionText && !diffQuestionTextExtended && !diffLicense && !diffVisibility && !diffSolution && !diffSolutionDescription && !diffSolutionMetadataJson && imageWasChanged === "false")
    {
        $('diffPanel_' + revId).hide();
        $('#noChangesAlert_' + revId).show();
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