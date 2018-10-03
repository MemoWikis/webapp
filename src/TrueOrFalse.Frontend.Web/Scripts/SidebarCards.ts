$(function() {
    $("#AllAutorsContainer").on('click', function () {  
        $("#ExtendAngle").toggleClass("rotate");
        if ($("#ExtendAngle").hasClass("rotate")) {
            $("#AllAutorsContainer").css('color', '#979797');
            $("#AllAutorsList").slideDown(400);

        } else {
            $("#AllAutorsContainer").css('color', '#0560AB');
            $("#AllAutorsList").slideUp(400);
        }
    });
});