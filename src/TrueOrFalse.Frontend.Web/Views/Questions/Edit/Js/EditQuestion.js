var EditQuestionForm = (function () {
    function EditQuestionForm() {
        new MarkdownQuestionExt();
        new MarkdownDescription();

        this.InitUpdateType();
        this.InitLicenseAgreement();

        $("form").submit(function (e) {
            $("#hddReferencesJson").val(ReferenceUi.ReferenceToJson());
        });

        $('.control-label .show-tooltip').append($("<span> <i class='fa fa-info-circle'></i></span>"));
    }
    EditQuestionForm.prototype.InitUpdateType = function () {
        function updateSolutionBody() {
            var selectedValue = $("#ddlAnswerType").val();
            $.ajax({
                url: $("#urlSolutionEditBody").val() + '?questionId=' + $("#questionId").val() + '&type=' + selectedValue,
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
    return EditQuestionForm;
})();

$(function () {
    new EditQuestionForm();
});
//# sourceMappingURL=EditQuestion.js.map
