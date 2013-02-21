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
    var $messages = $('#messages');
    $('#fileUpload').fineUploader({
        uploaderType: 'basic',
        button: $('#fileUpload')[0],
        request: {
            endpoint: $('#fileUpload').attr("data-endpoint")
        },
        multiple: false,
        debug: false,
        validation: {
            allowedExtensions: [
                'jpeg', 
                'jpg', 
                'png'
            ],
            sizeLimit: 2097152
        }
    }).on('error', function (event, id, filename, reason) {
        alert("Ein Fehler ist aufgetreten");
    }).on('complete', function (event, id, filename, responseJSON) {
        $("#divUploadProgress").hide();
        console.log(responseJSON);
    }).on('progress', function (event, id, filename, uploadedBytes, totalBytes) {
        $("#divUploadProgress").show();
        $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
    });
});
