/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
var categoryIdToDelete;
$(function () {
    $('a[href*=#modalDelete]').click(function () {
        categoryIdToDelete = $(this).attr("data-setId");
        populateDeleteCategory(categoryIdToDelete);
    });

    $('#btnCloseDelete').click(function () {
        $('#modalDelete').modal('hide');
    });

    $('#confirmDelete').click(function () {
        deleteCategory(categoryIdToDelete);
        $('#modalDelete').modal('hide');
    });
});

function populateDeleteCategory(setId) {
    $.ajax({
        type: 'POST',
        url: "/Categories/DeleteDetails/" + setId,
        cache: false,
        success: function (result) {
            $("#spanCategoryTitle").html(result.categoryTitle.toString());
        },
        error: function (result) {
            console.log(result);
            alert("Ein Fehler ist aufgetreten");
        }
    });
}

function deleteCategory(setId) {
    $.ajax({
        type: 'POST',
        url: "/Categories/Delete/" + setId,
        cache: false,
        success: function () {
            window.location.reload();
        },
        error: function (result) {
            console.log(result);
            alert("Ein Fehler ist aufgetreten");
        }
    });
}
//# sourceMappingURL=CategoriesDelete.js.map
