class SetDelete {
    constructor() {
        var setIdToDelete;

        $(function () {
            $('a[href*=#modalDelete]').click(function () {
                setIdToDelete = $(this).attr("data-setId");
                    populateDeleteSet(setIdToDelete);
            });

            $('#btnCloseSetDelete').click(function () {
                $('#modalDelete').modal('hide');
            });

            $('#confirmSetDelete').click(function () {
                if (typeof isEditMode === "undefined")
                    deleteSet(setIdToDelete);
                else
                    deleteSet(setIdToDelete, isEditMode);

                $('#modalDelete').modal('hide');
            });
        });
        
        function populateDeleteSet(setId) {
            $.ajax({
                type: 'POST',
                url: "/Sets/DeleteDetails/" + setId,
                cache: false,
                success: function (result) {
                    if (result.canNotBeDeleted) {
                        $("#setDeleteCanDelete").hide();
                        $("#setDeleteCanNotDelete").show();
                        $("#confirmSetDelete").hide();
                        $("#setDeleteCanNotDelete").html(result.canNotBeDeletedReason);
                        $("#btnCloseSetDelete").html("Schließen");
                    } else {
                        $("#setDeleteCanDelete").show();
                        $("#setDeleteCanNotDelete").hide();
                        $("#confirmSetDelete").show();
                        $("#spanSetTitle").html(result.setTitle.toString());
                        $("#btnCloseSetDelete").html("Abbrechen");
                    }
                },
                error(result) {
                    window.console.log(result);
                    window.alert("Ein Fehler ist aufgetreten");
                }
            });
        }

        function deleteSet(setId, isEdit = false) {
            $.ajax({
                type: 'POST',
                url: "/Sets/Delete/" + setId,
                cache: false,
                success: function() {
                    if (isEdit) {
                        window.alert("Das Set wurde erfolgreich gelöscht");
                        window.location.href = "/Fragesaetze";
                    }else
                        window.location.reload();
                },
                error: function (e) {
                    console.log(e);
                    window.alert("Ein Fehler ist aufgetreten");
                }
            });
        }
    }
}

