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
        $("#btnShowCategoriesWithNonAggregatedChildren").on("click",
            e => {
                e.preventDefault();
                self.RenderCategoriesWithNonAggregatedChildren();
            });
    }

    RenderLooseCategories() {
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

    RenderCategoriesWithNonAggregatedChildren() {
        $.ajax({
            type: 'POST',
            url: "/Maintenance/CmsShowCategoriesWithNonAggregatedChildren",
            cache: false,
            success: function (result) {
                $("#showCategoriesWithNonAggregatedChildrenResult").html(result);
            },
            error: function (result) {
                window.console.log(result);
                $("#showCategoriesWithNonAggregatedChildrenResult").html("<div class='alert alert-danger'>Ein Fehler ist aufgetreten.<br>" + result + "</div>");
            }
        });

    }

}