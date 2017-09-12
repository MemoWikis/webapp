class CategoryNetworkNavigation {

    constructor() {
        $("#categoryNetworkNavigationWrapper").on("click", '.networkNavigationUpdate', function (e) {
            e.preventDefault();
            var newCenterCategoryId = $(this).attr("data-category-id");
            $.get("/Categories/RenderCategoryNetworkNavigation/" + newCenterCategoryId,
                htmlResult => {
                    $("#categoryNetworkNavigation").html(htmlResult);
                });

        });
    }

}