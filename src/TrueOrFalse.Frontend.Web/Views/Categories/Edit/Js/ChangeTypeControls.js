$(function () {
    var isEditing = $("#isEditing").val() == "true";

    function updateTypeBody() {
        var selectedValue;
        if (isEditing) {
            selectedValue = $("#categoryType").val();
        } else {
            selectedValue = $("#ddlCategoryType").val();
        }

        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#categoryId").val() + '&type=' + selectedValue,
            type: 'GET',
            success: function (data) {
                $("#details-body").html(data);
            }
        });
    }

    if (!isEditing) {
        $("#ddlCategoryType").change(updateTypeBody);
    }

    updateTypeBody();
});
//# sourceMappingURL=ChangeTypeControls.js.map
