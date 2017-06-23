class EditCategoryNavBar {
    constructor() {
        $("#EditAggregationModal .nav .tab-unterthemen").click(e => {
            e.preventDefault();
            $("#EditAggregationModal .nav .tab-unterthemen").addClass("active");
            $("#EditAggregationModal .nav .tab-categories-graph").removeClass("active");
            $("#EditAggregationModal .tab-body").empty();
            $("#EditAggregationModal .tab-body").append("DATA FROM AJAX");
        });

        $("#EditAggregationModal .nav .tab-categories-graph").click(e => {
            e.preventDefault();
            $("#EditAggregationModal .nav .tab-categories-graph").addClass("active");
            $("#EditAggregationModal .nav .tab-unterthemen").removeClass("active");
            $("#EditAggregationModal .tab-body").empty();
            $("#EditAggregationModal .tab-body").append("DATA FROM AJAX");
        });
    }
}