class ChangeTypeControls {

    private _isCreating: boolean;

    constructor() {

        var self = this;
        this._isCreating = $("#isEditing").val() == "false";

        var ddlCategoryTypeMedia = $($("select[name='ddlCategoryTypeMedia']")[0]);
        var ddlCategoryTypeEducation = $($("select[name='ddlCategoryTypeEducation']")[0]);

        if (this._isCreating) {
            $("input[name=rdoCategoryTypeGroup]:radio").change(function() { self.UpdateTypeBody(); });
            ddlCategoryTypeMedia.change(function () { self.UpdateTypeBody(); });
            ddlCategoryTypeEducation.change(function () { self.UpdateTypeBody(); });

            self.InitGroupBehaviour();

            $("input:radio[name='rdoCategoryTypeGroup']:checked").trigger("change");
            var selectedRdo = $($("input:radio[name='rdoCategoryTypeGroup']:checked")[0]);

            if (selectedRdo.val() == "media") {
                ddlCategoryTypeMedia.val(ddlCategoryTypeMedia.attr("data-selectedValue"));
                ddlCategoryTypeMedia.trigger("change");
            }else if (selectedRdo.val() == "education") {
                ddlCategoryTypeEducation.val(ddlCategoryTypeEducation.attr("data-selectedValue"));
                ddlCategoryTypeEducation.trigger("change");
            } 
        } else {
            this.UpdateTypeBody();
        }
    }


    UpdateTypeBody() {

        var selectedValue;
        if (!this._isCreating) {
            selectedValue = $("#categoryType").val();
        } else {
            
        if ($("input:radio[name=rdoCategoryTypeGroup]:checked").val() == 'standard')
                selectedValue = 'standard';
        if ($("input:radio[name=rdoCategoryTypeGroup]:checked").val() == 'media')
                selectedValue = $("select[name=ddlCategoryTypeMedia]").val();
        if ($("input:radio[name='rdoCategoryTypeGroup']:checked").val() == 'education')
            selectedValue = $("select[name='ddlCategoryTypeEducation']").val();
        }
                                            
        $("#CategoryDetailsBody").html("<h4 class='CategoryTypeHeader'> Formular wird geladen...</h4>");


        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#categoryId").val() + '&type=' + selectedValue,
            type: 'GET',
            success: function(data) {
                $("#CategoryDetailsBody").html(data);
                $('#CategoryDetailsBody .show-tooltip').tooltip();
                fnEditCatValidation(selectedValue);
            }
        });

        
    }

    InitGroupBehaviour() {
        $("input:radio[name='rdoCategoryTypeGroup']").change(function () {
            $("input:radio[name='rdoCategoryTypeGroup']").parent().children("select").hide();
            $("input:radio[name='rdoCategoryTypeGroup']:checked").parent().children("select").slideDown();
        });
    }
}



$(function () {
    new ChangeTypeControls();
});
