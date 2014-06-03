
class bla {

    private _isCreating: boolean;

    constructor() {

        var self = this;
        this._isCreating = $("#isEditing").val() == "false";
        
        if (this._isCreating) {
            $("#ddlCategoryType").change(self.UpdateTypeBody);
            self.InitGroupBehaviour();
        }

        this.UpdateTypeBody();
    }


    UpdateTypeBody() {

        var selectedValue;
        if (this._isCreating) {
            selectedValue = $("#categoryType").val();
        } else {

            //selectedValue = $("#ddlCategoryType").val();
        }

        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#categoryId").val() + '&type=' + selectedValue,
            type: 'GET',
            success: function(data) { $("#CategoryDetailsBody").html(data); }
        });
    }

    InitGroupBehaviour() {
        $("input:radio[name=rdoCategoryTypeGroup]").change(function () {
            $("input:radio[name=rdoCategoryTypeGroup]").parent().children("select").hide();
            $("input:radio[name=rdoCategoryTypeGroup]:checked").parent().children("select").slideDown();
        });
    }
}



$(function () {
    new bla();
});
