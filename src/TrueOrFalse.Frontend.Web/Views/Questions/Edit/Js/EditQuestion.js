var EditQuestionForm = (function () {
    function EditQuestionForm() {
        new MarkdownQuestionExt();
        new MarkdownDescription();

        this.InitUpdateType();
        this.InitLicenseAgreement();
        this.InitButtonTextUpdate();

        $("form").submit(function (e) {
            $("#hddReferencesJson").val(ReferenceUi.ReferenceToJson());
        });

        $('.control-label .show-tooltip').append($("<span> <i class='fa fa-info-circle'></i></span>"));

        $('[name="Visibility"]').trigger('change');

        $(window).trigger('referencesChanged');

        $("#ConfirmContentRights").prop("checked", false);
    }
    EditQuestionForm.prototype.InitUpdateType = function () {
        function updateSolutionBody() {
            var selectedValue = $("#ddlAnswerType").val();
            var questionId = -1;
            if ($("#isEditing").val() === "True")
                questionId = $("#questionId").val();

            $.ajax({
                url: $("#urlSolutionEditBody").val() + '?questionId=' + questionId + '&type=' + selectedValue,
                type: 'GET',
                success: function (data) {
                    $("#answer-body").html(data);
                }
            });
        }
        $("#ddlAnswerType").change(updateSolutionBody);
        updateSolutionBody();
    };

    EditQuestionForm.prototype.InitLicenseAgreement = function () {
        $('[name="Visibility"]').change(function () {
            if ($('input[name="Visibility"]:checked').val() == "Owner") {
                $('#Agreement').hide();
            } else {
                $('#Agreement').show();
            }
        });
    };

    EditQuestionForm.prototype.InitButtonTextUpdate = function () {
        $(window).bind('referencesChanged', function () {
            if ($('#JS-References .JS-ReferenceContainer[id^="Ref-"]').length == 0) {
                $('#AddReference').html('Eine Quelle hinzufügen');
            } else {
                $('#AddReference').html('Weitere Quelle hinzufügen');
            }
        });
    };
    return EditQuestionForm;
})();

$(function () {
    new EditQuestionForm();
});
//# sourceMappingURL=EditQuestion.js.map
