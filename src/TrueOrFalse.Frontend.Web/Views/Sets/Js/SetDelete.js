var categoryIdToDelete;
$(function () {
    $('a[href*=#modalDelete]').click(function () {
        categoryIdToDelete = $(this).attr("data-setId");
        populateDeleteSet(categoryIdToDelete);
    });

    $('#btnCloseDelete').click(function () {
        $('#modalDelete').modal('hide');
    });

    $('#confirmDelete').click(function () {
        deleteSet(categoryIdToDelete);
        $('#modalDelete').modal('hide');
    });
});

function populateDeleteSet(setId) {
    $.ajax({
        type: 'POST',
        url: "/Sets/DeleteDetails/" + setId,
        cache: false,
        success: function (result) {
            $("#spanSetTitle").html(result.setTitle);
        },
        error: function () {
            window.alert("Ein Fehler ist aufgetreten");
        }
    });
}

function deleteSet(setId) {
    $.ajax({
        type: 'POST',
        url: "/Sets/Delete/" + setId,
        cache: false,
        success: function (result) {
            if (result.IsPartOfDate) {
                window.alert("Der Fragesatz kann nicht gelöscht werden, da er in einem Termin verwendet wird.");
            } else if (!result.Success) {
                window.alert("Ein Fehler ist aufgetreten");
            }
            window.location.reload();
        },
        error: function (result) {
            window.console.log(result);
            window.alert("Ein Fehler ist aufgetreten");
        }
    });
}
//# sourceMappingURL=SetDelete.js.map
