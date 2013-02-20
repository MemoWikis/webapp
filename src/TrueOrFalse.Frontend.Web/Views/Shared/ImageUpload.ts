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

    $('#jquery-wrapped-fine-uploader').fineUploader({
          request: {
            endpoint: '/QuestionSet/UploadImage/'
          }
    });
});
