$(function() {
    function updateTypeBody() {
        var selectedValue = $("#ddlCategoryType").val();
        $.ajax({
            url: '/EditCategory/DetailsPartial?categoryId=' + $("#categoryId").val() + '&type=' + selectedValue,
            type: 'GET',
            success: function (data) { $("#details-body").html(data); }
        });
    }

    $("#ddlCategoryType").change(updateTypeBody);
    updateTypeBody();
});
