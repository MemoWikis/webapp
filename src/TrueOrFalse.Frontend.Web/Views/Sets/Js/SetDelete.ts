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
        success: result => {
            $("#spanSetTitle").html(result.setTitle);
        },
        error(result) {
            window.console.log(result);
            window.alert("Ein Fehler ist aufgetreten");
        }
    });
}

function deleteSet(setId) {
    $.ajax({
        type: 'POST',
        url: "/Sets/Delete/" + setId,
        cache: false,
        success: (result) => {
            if (result.IsPartOfDate) {
                window.alert("Der Fragesatz kann nicht gelöscht werden, da er in einem Termin verwendet wird.");
            }
            else if (!result.Success) {
                window.alert("Ein Fehler ist aufgetreten");
            }
            window.location.reload();
        },
        error(result) {
            window.console.log(result);
            window.alert("Ein Fehler ist aufgetreten");
        }
    });
}