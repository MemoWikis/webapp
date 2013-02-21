/// <reference path="../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../Scripts/typescript.defs/fineUploader.d.ts" />

$(function () {
    $("#rdoImageWikipedia").change(function () {
        if ($(this).is(':checked')) {
            $("#divUpload").hide();
            $("#divWikimedia").show();
        }
    });
    
    $("#rdoImageUpload").change(function () {
        if ($(this).is(':checked')) {
            $("#divUpload").show();
            $("#divWikimedia").hide();
        }
    });

    var $messages = $('#messages');
    $('#fileUpload').fineUploader({
        uploaderType: 'basic',
        button : $('#fileUpload')[0],
        request: { endpoint: $('#fileUpload').attr("data-endpoint") },
        multiple: false,
        debug: false,
        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'png'],
            sizeLimit: 2097152 // 2MB = 2048 * 1024 bytes
        }
    })
    .on('error', function(event, id, filename, reason) {
        alert("Ein Fehler ist aufgetreten");
    })
    .on('complete', function(event, id, filename, responseJSON){
        $("#divUploadProgress").hide();
        console.log(responseJSON);
    })
    .on('progress', function(event, id, filename, uploadedBytes : number, totalBytes : number){
        $("#divUploadProgress").show();
        $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
    });
});