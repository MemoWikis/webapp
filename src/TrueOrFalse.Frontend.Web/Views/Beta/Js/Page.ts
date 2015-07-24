$(() => {
    $(".shake")
        .hover(
            function () { $(this).addClass("tada animated"); },
            function () { $(this).removeClass("tada animated"); }
        );

    $("#txtBetaCode").focus();
});
