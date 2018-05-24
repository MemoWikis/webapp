class CategoryDelete {

    constructor() {
        var categoryIdToDelete;
        var self = this;

        $(document).on("click", 'a[href*=#modalDeleteCategory]', function (e) {
            categoryIdToDelete = $(this).attr("data-categoryId");
         
        });

        $('#btnCloseDelete').click(function (e) {
            $('#modalDeleteCategory').modal('hide');
            e.preventDefault();
        });

        $('#confirmDelete').click(function (e) {
            self.DeleteCategory(categoryIdToDelete);
            $('#modalDeleteCategory').modal('hide');
            $(".btn").attr("disabled", "disabled");
            $('#deleteAlert').fadeIn();
            e.preventDefault();

        });
    }

    DeleteCategory(catId) {
        $.ajax({
            type: 'POST',
            url: "/Categories/Delete/" + catId,
            cache: false,
            success: () => {
                window.alert("Das Thema wurde erfolgreich gelöscht.");
                window.location.href = "/Kategorien";
            },
            error: (result) => {
                window.console.log(result);
                window.alert("Ein Fehler ist aufgetreten");
            }
        });
    }
}
