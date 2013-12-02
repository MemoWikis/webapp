/// <reference path="../../../../Scripts/SolutionMetaData.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />

class SolutionMetaDataMenu {

    _divMenu: JQuery;
    _divMenuItemText: JQuery;
    _divMenuItemNumber: JQuery;
    _divMenuItemDate: JQuery;

    _sliderDate: SliderDate;
    _numberAccuracy: NumberAccuracy;

    constructor () {

        this._sliderDate = new SliderDate();
        this._numberAccuracy = new NumberAccuracy();

        $("#btnMenuItemText").click(this.SelectText);
        $("#btnMenuItemText, #divMenuItemText").hover(
            function () { $("#divMenuItemText").show();},
            function () { $("#divMenuItemText").hide();}
        );

        $("#btnMenuItemNumber").click(this.SelectNumber);
        $("#btnMenuItemNumber, #divMenuItemNumber").hover(
            function () { $("#divMenuItemNumber").show(); },
            function () { $("#divMenuItemNumber").hide(); }
        );

        $("#btnMenuItemDate").click(this.SelectDate);
        $("#btnMenuItemDate, #divMenuItemDate").hover(
            function () { $("#divMenuItemDate").show(); },
            function () { $("#divMenuItemDate").hide(); }
        );
	}

    SelectText() 
    { 
        var obj = <SolutionMetaData>jQuery.parseJSON('{"Name":"John"}');
        console.log(obj.Name);
        $("#btnMenuItemText").addClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").removeClass("active");
    }
    
    SelectNumber() 
    { 
        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").addClass("active");
        $("#btnMenuItemDate").removeClass("active");
    }

    SelectDate()
    {
        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").addClass("active");
    }
}

class SliderDate
{ 
    constructor() {
        var _this = this;
        $("#sliderDate").slider({
            range: "min",
            max: 2,
            value: 0,
            slide: function (event, ui) { _this.SetUiSlider(ui.value); },
            change: function (event, ui) { _this.SetUiSlider(ui.value); }
        });
    }

    SetUiSlider(sliderValue) {        
        var text = "";
        if (sliderValue == 0)
            text = "Tag";
        else if (sliderValue == 1)
            text = "Monat";
        else if (sliderValue == 2)
            text = "Jahr";

        $("#spanSliderValue").text(text);
    }
}

class NumberAccuracy
{ 
    constructor() {     
        $("#numberAccuracy").change(function () {
            var newVal = $(this).val().replace(/[^0-9]/g, '');
            $(this).val(newVal);
        });
    }
}

var solutionMetaData = new SolutionMetaDataMenu();

$('#help').click(function () {
    $("#modalHelpSolutionType").modal('show');
});
