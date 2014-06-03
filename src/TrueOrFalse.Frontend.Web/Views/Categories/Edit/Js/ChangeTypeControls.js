var bla = (function () {
    function bla() {
        var self = this;
        this._isCreating = $("#isEditing").val() == "false";

        if (this._isCreating) {
            $("#ddlCategoryType").change(self.UpdateTypeBody);
            self.InitGroupBehaviour();
        }

        this.UpdateTypeBody();
    }
    bla.prototype.UpdateTypeBody = function () {
        var selectedValue;
        if (this._isCreating) {
            selectedValue = $("#categoryType").val();
        } else {
            //selectedValue = $("#ddlCategoryType").val();
        }

        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#categoryId").val() + '&type=' + selectedValue,
            type: 'GET',
            success: function (data) {
                $("#CategoryDetailsBody").html(data);
            }
        });
    };

    bla.prototype.InitGroupBehaviour = function () {
        $("input:radio[name=rdoCategoryTypeGroup]").change(function () {
            $("input:radio[name=rdoCategoryTypeGroup]").parent().children("select").hide();
            $("input:radio[name=rdoCategoryTypeGroup]:checked").parent().children("select").slideDown();
        });
    };
    return bla;
})();

$(function () {
    new bla();
});
//# sourceMappingURL=ChangeTypeControls.js.map
