$(function () {
    new AutocompleteSets("#txtSet");


    $("#EditDateForm").submit(function () {
        $("#btnSave").html("<i class=\"fa fa-spinner fa-spin\">&nbsp;</i>" + $("#btnSave").html());
        $("#btnSave").prop("disabled", true);
        return true;
    });
}); 
