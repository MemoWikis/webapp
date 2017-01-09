class CategoryDelete {


    constructor() {
        var categoryIdToDelete;
        var self = this;

        $('a[href*=#modalDeleteCategory]').click(function () {
            categoryIdToDelete = $(this).attr("data-categoryId");
            self.PopulateDeleteCategory(categoryIdToDelete);
        });

        $('#btnCloseDelete').click(function (e) {
            $('#modalDeleteCategory').modal('hide');
            e.preventDefault();
        });

        $('#confirmDelete').click(function (e) {
            self.DeleteCategory(categoryIdToDelete);
            $('#modalDeleteCategory').modal('hide');
            e.preventDefault();
        });
    }

    PopulateDeleteCategory(catId) {
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

    DeleteCategory(catId) {
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
    
}
