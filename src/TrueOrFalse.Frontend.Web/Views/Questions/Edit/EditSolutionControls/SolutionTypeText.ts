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

    _current: SolutionMetaData;

    constructor () {

        this._sliderDate = new SliderDate();
        this._numberAccuracy = new NumberAccuracy();

        var jsonMetaData = this.GetJsonMetaData();
        if (jsonMetaData != null) {
            var solutionMetaData = <SolutionMetaData>jsonMetaData;
            if (solutionMetaData.IsDate)
                this.SelectDate();
            else if (solutionMetaData.IsNumber)
                this.SelectNumber();
            else if (solutionMetaData.IsText)
                this.SelectText();
        } else {
            this.SelectText();
        }
        
        $("#btnMenuItemText").click(() => { this.SelectText(); $("#divMenuItemText").show(); });
        $("#btnMenuItemText, #divMenuItemText").hover(
            () => { if (this._current.IsText) $("#divMenuItemText").show(); },
            () => { $("#divMenuItemText").hide();}
        );

        $("#btnMenuItemNumber").click(() => { this.SelectNumber(); $("#divMenuItemNumber").show(); });
        $("#btnMenuItemNumber, #divMenuItemNumber").hover(
            () => { if (this._current.IsNumber) $("#divMenuItemNumber").show(); },
            () => { $("#divMenuItemNumber").hide(); }
        );

        $("#btnMenuItemDate").click(() => { this.SelectDate(); $("#divMenuItemDate").show(); });
        $("#btnMenuItemDate, #divMenuItemDate").hover(
            () => { if (this._current.IsDate) $("#divMenuItemDate").show(); },
            () => { $("#divMenuItemDate").hide(); }
        );
	}

    SelectText() 
    {
        this.ResetAll();
        $("#btnMenuItemText").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataText());
    }
    
    SelectNumber() 
    {
        this.ResetAll();
        $("#btnMenuItemNumber").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataNumber());
    }

    SelectDate()
    {
        this.ResetAll();
        $("#infoMetaDate").show();
        $("#btnMenuItemDate").addClass("active");

        var metaData = this.GetJsonMetaData();
        if (metaData != null && metaData.IsDate) {
            metaData = <SolutionMetadataDate>metaData;
            $("#sliderDate").val(metaData.Precision);
            this.SetJsonMetaData(metaData);
        } else {
            this.SetJsonMetaData(new SolutionMetadataDate());
        }
    }

    ResetAll() {
        $("#infoMetaDate").hide();

        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").removeClass("active");
    }

    GetJsonMetaData(): any{
        var jsonVal = $("#MetadataSolutionJson").val();
        if (jsonVal.length == 0)
            return null;

        return jQuery.parseJSON(jsonVal);
    }

    SetJsonMetaData(json: any) {
        this._current = json;
        $("#MetadataSolutionJson").val(JSON.stringify(json));
    }

}

class SliderDate
{ 
    constructor() {
        var _this = this;
        $("#sliderDate").slider({
            range: "min",
            value: 3,
            min: 1,
            max: 6,
            slide: function (event, ui) { _this.SetUiSlider(ui.value); },
            change: function (event, ui) { _this.SetUiSlider(ui.value); }
        });
    }

    SetUiSlider(sliderValue) {        
        var text = "";
        if (sliderValue == 1) {
            text = "Tag";
        }
        else if (sliderValue == 2){
            text = "Monat";
        }
        else if (sliderValue == 3){
            text = "Jahr";
        }
        else if (sliderValue == 4){
            text = "Jahrzent";
        }
        else if (sliderValue == 5){
            text = "Jahrhundert";
        }
        else if (sliderValue == 6){
            text = "Jahrtausend";
        }

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
