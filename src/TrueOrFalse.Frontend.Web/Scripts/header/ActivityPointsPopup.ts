$(() => {

    $("#header-level-display-popover").popover({
        html: true,
        container: '#MasterHeader', //to circumvent restrictions in width of parent element
        content: function() {
            var htmlContent = "";
            $.ajax({
                url: "/Shared/RenderActivityPopupContent",
                type: 'GET',
                async: false,
                success: function(htmlResult) {
                    htmlContent = htmlResult;
                }
            });

            return htmlContent;
        },
    });

});