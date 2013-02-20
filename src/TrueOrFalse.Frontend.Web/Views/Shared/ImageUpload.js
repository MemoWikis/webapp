$(function () {
    $("#rdoImageWikipedia").change(function () {
        if($(this).is(':checked')) {
            $("#divUpload").hide();
            $("#divWikimedia").show();
        }
    });
    $("#rdoImageUpload").change(function () {
        if($(this).is(':checked')) {
            $("#divUpload").show();
            $("#divWikimedia").hide();
        }
    });
});
