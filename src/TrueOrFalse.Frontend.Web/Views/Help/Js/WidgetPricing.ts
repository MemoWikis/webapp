$(function () {
    new WidgetPricing();
});

class WidgetPricing {

    constructor() {
        var self = this;

        $("#btnPricingIndividual")
            .click((e) => {
                e.preventDefault();
                $("#btnPricingIndividual").addClass("btn-primary").removeClass("btn-notSelected");
                $("#btnPricingOrgs").removeClass("btn-primary").addClass("btn-notSelected");
                $("#pricingIndividuals").show(0);
                $("#pricingOrgs").hide(0);
            });

        $("#btnPricingOrgs")
            .click((e) => {
                e.preventDefault();
                $("#btnPricingOrgs").addClass("btn-primary").removeClass("btn-notSelected");
                $("#btnPricingIndividual").removeClass("btn-primary").addClass("btn-notSelected");
                $("#pricingIndividuals").hide(0);
                $("#pricingOrgs").show(0);
            });

    }

}