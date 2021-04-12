class ChangeTypeControls {

    private _isCreating: boolean;

    constructor() {

        var self = this;
        this._isCreating = $("#isEditing").val() == "false";

        var ddlCategoryTypeMedia = $($("select[name='ddlCategoryTypeMedia']")[0]);
        var ddlCategoryTypeEducation = $($("select[name='ddlCategoryTypeEducation']")[0]);

        if (this._isCreating) {
            $("input[name=rdoCategoryTypeGroup]:radio").change(function (e, changeControlsOnly = false, type?: string) {
                if (!changeControlsOnly)
                    self.UpdateTypeBody(type);
            });
            ddlCategoryTypeMedia.change(function(e, type) {
                self.UpdateTypeBody(type);
            });
            ddlCategoryTypeEducation.change(function () { self.UpdateTypeBody(); });

            //Rendering of initial type partial is triggered by change event in script rendered in the view

            self.InitGroupBehaviour();


            var selectedRdo = $($("input:radio[name='rdoCategoryTypeGroup']:checked")[0]);

            if (selectedRdo.val() == "media") {
                ddlCategoryTypeMedia.val(ddlCategoryTypeMedia.attr("data-selectedValue"));
                ddlCategoryTypeMedia.trigger("change");
            } else if (selectedRdo.val() == "education") {
                ddlCategoryTypeEducation.val(ddlCategoryTypeEducation.attr("data-selectedValue"));
                ddlCategoryTypeEducation.trigger("change");
            }


        } else {
            this.UpdateTypeBody();
        }
    }

    UpdateTypeBody(type?: string) {

        var selectedValue = "";
        if (type) {
            selectedValue = type;
        } else {
            if (!this._isCreating) {
                selectedValue = $("#categoryType").val();
            } else {
            
                if ($("input:radio[name=rdoCategoryTypeGroup]:checked").val() == 'standard')
                        selectedValue = 'Standard';
                if ($("input:radio[name=rdoCategoryTypeGroup]:checked").val() == 'media')
                        selectedValue = $("select[name=ddlCategoryTypeMedia]").val();
                if ($("input:radio[name='rdoCategoryTypeGroup']:checked").val() == 'education')
                    selectedValue = $("select[name='ddlCategoryTypeEducation']").val();
            }
        }
                                            
        $('.JS-ShowWithPartial').hide();
        $("#CategoryDetailsBody").html("<h4 class='CategoryTypeHeader'> Formular wird geladen...</h4>");

        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#hhdCategoryId").val() + '&type=' + 'Standard',
            type: 'GET',
            success: function (data) {
                fnEditCatValidation(selectedValue, true, false);//initiate validator without rules for fields not rendered yet
                $("#CategoryDetailsBody").html(data);
                $('.JS-ShowWithPartial').show();
                $('#CategoryDetailsBody .show-tooltip').tooltip();
                fnEditCatValidation(selectedValue, false, true);
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
