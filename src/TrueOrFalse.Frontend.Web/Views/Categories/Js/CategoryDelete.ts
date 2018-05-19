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
            self.SafeDeleteCategory(435,443);
            self.DeleteCategory(categoryIdToDelete);
            $('#modalDeleteCategory').modal('hide');
            e.preventDefault();

        });
    }

    PopulateDeleteCategory(catId) {
        //$.ajax({
        //    type: 'POST',
        //    url: "/Categories/DeleteDetails/" + catId,
        //    cache: false,
        //    success: function (result) {
        //        $("#spanCategoryTitle").html(result.categoryTitle.toString());
        //    },
        //    error: function (result) {
        //        window.console.log(result);
        //        window.alert("Ein Fehler ist aufgetreten");
        //    }
        //});
    }

    DeleteCategory(catId) {
        $.ajax({
            type: 'POST',
            url: "/Categories/SetDeleteMarker/" + catId,
            cache: false,
            success(data) {
                $.ajax({
                    type: 'POST',
                    url: "/Categories/Delete/" + catId,
                    cache: false,
                    success: function () {
                        window.alert("Das Thema wurde erfolgreich gelöscht.");
                        window.location.href = "/Kategorien";
                    },
                    error: function (result) {
                        window.console.log(result);
                        window.alert("Ein Fehler ist aufgetreten");
                    }
                });
            },
            error(xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }

        });


    }

    SafeDeleteCategory(catId,userID) {
       
        console.log("Hallo Daniel");
           
      
    }
    
}
