/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />

var categoryIdToDelete;
$(function () {
    $('a[href*=#modalDelete]').click(function () {
        categoryIdToDelete = $(this).attr("data-categoryId");
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

function populateDeleteCategory(catId) {
    $.ajax({
        type: 'POST',
        url: "/Categories/DeleteDetails/" + catId,
        cache: false,
        success: function (result) {
            $("#spanCategoryTitle").html(result.categoryTitle.toString());
        },
        error: function (result) {
            window.console.log(result);
            window.alert("Ein Fehler ist aufgetreten");
        }
    });
}

function deleteCategory(catId) {
    $.ajax({
        type: 'POST',
        url: "/Categories/Delete/" + catId,
        cache: false,
        success: function () { window.location.reload(); },
        error: function (result) {
            window.console.log(result);
            window.alert("Ein Fehler ist aufgetreten");
        }
    });
}