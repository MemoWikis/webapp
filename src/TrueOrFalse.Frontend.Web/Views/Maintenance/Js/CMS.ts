$(() => {
    new Cms();
});


class Cms {

    constructor() {
        var self = this;
        $("#btnShowLooseCategories").on("click",
            e => {
                e.preventDefault();
                self.RenderLooseCategories();
            });
    }

    RenderLooseCategories() {
        console.log("rendering...");
        $.ajax({
            type: 'POST',
            url: "/Maintenance/CmsShowLooseCategories",
            cache: false,
            success: function (result) {
                $("#showLooseCategoriesResult").html(result);
            },
            error: function (result) {
                window.console.log(result);
                $("#showLooseCategoriesResult").html("<div class='alert alert-danger'>Ein Fehler ist aufgetreten.<br>" + result + "</div>");
            }
        });

    }

}