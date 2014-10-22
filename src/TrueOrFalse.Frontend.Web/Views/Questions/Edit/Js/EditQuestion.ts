
class EditQuestionForm
{
    constructor() {

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

    public InitButtonTextUpdate() {
        $(window).bind('referencesChanged', function () {
            if ($('#JS-References .JS-ReferenceContainer[id^="Ref-"]').length == 0) {
                $('#AddReference').html('Eine Quelle hinzufügen');
            } else {
                $('#AddReference').html('Weitere Quelle hinzufügen');
            }
            
        });
    }

}


$(function () {
    new EditQuestionForm();
});