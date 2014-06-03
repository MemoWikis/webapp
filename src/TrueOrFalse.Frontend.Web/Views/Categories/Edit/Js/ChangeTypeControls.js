var bla = (function () {
    function bla() {
        var self = this;
        this._isCreating = $("#isEditing").val() == "false";

        if (this._isCreating) {
            $("input[name=rdoCategoryTypeGroup]:radio").change(function () {
                self.UpdateTypeBody();
            });
            $("select[name='ddlCategoryTypeMedia']").change(function () {
                self.UpdateTypeBody();
            });
            $("select[name='ddlCategoryTypeEducation']").change(function () {
                self.UpdateTypeBody();
            });

            self.InitGroupBehaviour();
        }

        this.UpdateTypeBody();
    }
    bla.prototype.UpdateTypeBody = function () {
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
            success: function (data) {
                $("#CategoryDetailsBody").html(data);
            }
        });
    };

    bla.prototype.InitGroupBehaviour = function () {
        $("input:radio[name='rdoCategoryTypeGroup']").change(function () {
            $("input:radio[name='rdoCategoryTypeGroup']").parent().children("select").hide();
            $("input:radio[name='rdoCategoryTypeGroup']:checked").parent().children("select").slideDown();
        });
    };
    return bla;
})();

$(function () {
    new bla();
});
//# sourceMappingURL=ChangeTypeControls.js.map
