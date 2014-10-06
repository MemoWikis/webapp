
class EditQuestionForm
{
    constructor() {

        new MarkdownQuestionExt();
        new MarkdownDescription();

        this.InitUpdateType();
        this.InitLicenseAgreement();

        $("form").submit(function (e) {
            $("#hddReferencesJson").val(ReferenceUi.ReferenceToJson());
        });

        $('.control-label .show-tooltip').append($("<span> <i class='fa fa-info-circle'></i></span>"));
    }

    public InitUpdateType() {
        function updateSolutionBody() {
            var selectedValue = $("#ddlAnswerType").val();
            $.ajax({
                url: $("#urlSolutionEditBody").val() + '?questionId=' + $("#questionId").val() + '&type=' + selectedValue,
                type: 'GET',
                success: function (data) { $("#answer-body").html(data); }
            });
        }
        $("#ddlAnswerType").change(updateSolutionBody);
        updateSolutionBody();        
    }

    public InitLicenseAgreement() {
        $('[name="Visibility"]').change(function () {
            if ($('input[name="Visibility"]:checked').val() == "Owner") {
                $('#Agreement').hide();
            } else {
                $('#Agreement').show();
            }
        });        
    }

}


$(function () {
    new EditQuestionForm();
});