
class bla {

    private _isCreating: boolean;

    constructor() {

        var self = this;
        this._isCreating = $("#isEditing").val() == "false";
        
        if (this._isCreating) {
            $("input[name=rdoCategoryTypeGroup]:radio").change(function() { self.UpdateTypeBody(); });
            $("select[name='ddlCategoryTypeMedia']").change(function () { self.UpdateTypeBody(); });
            $("select[name='ddlCategoryTypeEducation']").change(function () { self.UpdateTypeBody(); });

            self.InitGroupBehaviour();
        }

        this.UpdateTypeBody();
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

        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#categoryId").val() + '&type=' + selectedValue,
            type: 'GET',
            success: function(data) { $("#CategoryDetailsBody").html(data); }
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
    new bla();
});
