var setIdToDelete;
$(function () {
    $('a[href*=#modalDelete]').click(function () {
        setIdToDelete = $(this).attr("data-setId");
        populateDeleteSet(setIdToDelete);
    });
    $('#btnCloseDelete').click(function () {
        $('#modalDelete').modal('hide');
    });
    $('#confirmDelete').click(function () {
        deleteSet(setIdToDelete);
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
        success: function () {
            window.location.reload();
        },
        error: function (result) {
            console.log(result);
            alert("Ein Fehler ist aufgetreten");
        }
    });
}
