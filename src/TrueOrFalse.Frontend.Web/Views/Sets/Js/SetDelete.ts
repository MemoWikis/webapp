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
            alert("Ein Fehler ist aufgetreten");
        }
    });
}

function deleteSet(setId) {
    $.ajax({
        type: 'POST',
        url: "/Sets/Delete/" + setId,
        cache: false,
        success: function () { window.location.reload(); },
        error: function (result) {
            console.log(result);
            alert("Ein Fehler ist aufgetreten");
        }
    });
}