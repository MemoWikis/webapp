class CategoryDelete {


    constructor() {
        var categoryIdToDelete;
        var self = this;

        $(document).on("click", 'a[href*=#modalDeleteCategory]', function (e) {
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
            $('#forTheTimeToDeleteModal').modal('show');
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
                $('#forTheTimeToDeleteModal').modal('hide');
            }
        });
    }

    DeleteCategory(catId) {
        $.ajax({
            type: 'POST',
            url: "/Categories/Delete/" + catId,
            cache: false,
            success: function () {
                $('#forTheTimeToDeleteModal').modal('hide');
                window.location.href = "/Kategorien";
            },
            error: function (result) {
                window.console.log(result);
            }
        });
    }
    
}
