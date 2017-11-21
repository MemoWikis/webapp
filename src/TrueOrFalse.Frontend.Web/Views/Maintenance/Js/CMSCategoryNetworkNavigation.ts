class CmsCategoryNetworkNavigation {

    constructor() {
        $("#categoryNetworkNavigationWrapper").on("click", '.networkNavigationUpdate', function (e) {
            e.preventDefault();
            var newCenterCategoryId = $(this).attr("data-category-id");
            $.get("/Maintenance/CmsRenderCategoryNetworkNavigation/" + newCenterCategoryId,
                htmlResult => {
                    $("#categoryNetworkNavigation").html(htmlResult);
                });

        });
    }

}