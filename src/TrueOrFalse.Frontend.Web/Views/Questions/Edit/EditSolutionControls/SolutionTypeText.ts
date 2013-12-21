/// <reference path="../../../../Scripts/MM.DateParser.ts" />
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

        this._sliderDate = new SliderDate(this.SetJsonMetaData);
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

        $("#Answer").keyup(() => {
            console.log(this._current);
            if (this._current.IsDate) {
                this._sliderDate.SetDateUi();
            }
        });
        
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
        $("#infoMetaText").show();
        $("#btnMenuItemText").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataText());
    }
    
    SelectNumber() 
    {
        this.ResetAll();
        $("#infoMetaNumber").show();
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
            this._sliderDate.Set(metaData);
        } 

        this.SetJsonMetaData(this._sliderDate.MetaData);
    }

    ResetAll() {
        $("#infoMetaDate").hide();
        $("#infoMetaText").hide();
        $("#infoMetaNumber").hide();

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
    MetaData: SolutionMetadataDate = new SolutionMetadataDate();
    SaveJson: (json: any) => any;

    _slider: any;

    constructor(SaveJson: (json: any) => any) {
        this.SaveJson = SaveJson;
        var _this = this;
        this._slider = $("#sliderDate").slider({
            range: "min",
            value: 3,
            min: 1,
            max: 6,
            slide: function (event, ui) { _this.SetUiSlider(ui.value); },
            change: function (event, ui) { _this.SetUiSlider(ui.value); }
        });
    }

    public Set(metaData: SolutionMetadataDate) {
        this.MetaData = metaData;
        this._slider.slider("value", this.MetaData.Precision);
        this.SetDateUi();
    }

    SetUiSlider(sliderValue) {
        if (sliderValue == 1) this.MetaData.Precision = DatePrecision.Day;
        else if (sliderValue == 2) this.MetaData.Precision = DatePrecision.Month;
        else if (sliderValue == 3) this.MetaData.Precision = DatePrecision.Year;
        else if (sliderValue == 4) this.MetaData.Precision = DatePrecision.Decade;
        else if (sliderValue == 5) this.MetaData.Precision = DatePrecision.Century;
        else if (sliderValue == 6) this.MetaData.Precision = DatePrecision.Millenium;

        this.SetDateUi();
    }

    SetDateUi() {
        console.log(this.MetaData);

        var text = SolutionMetadataDate.GetPrecisionLabel(this.MetaData.Precision);

        var dateR = DateParser.Run($("#Answer").val());
        console.log(dateR);
        if (!dateR.IsValid) {
            $("#spanEntryPrecision").html("keine g&#252;ltige Eingabe");
            $("#iDateError").show();
            $("#iDateCorrect").hide();
        }
        else {
            $("#spanEntryPrecision").html(
                "<b>" + SolutionMetadataDate.GetPrecisionLabel(dateR.Precision) + "genau </b>" +
                "(" + dateR.ToLabel() + ")"
            );
            $("#iDateError").hide();
            $("#iDateCorrect").show();
        }


        this.SaveJson(this.MetaData);
        $("#spanAnswerPrecision").text(text);
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

$('.show-tooltip').tooltip();

$('#help').click(function () {
    $("#modalHelpSolutionType").modal('show');
});