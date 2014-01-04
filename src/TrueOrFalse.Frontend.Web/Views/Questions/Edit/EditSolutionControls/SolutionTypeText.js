/// <reference path="../../../../Scripts/MM.DateParser.ts" />
/// <reference path="../../../../Scripts/SolutionMetaData.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
var SolutionMetaDataMenu = (function () {
    function SolutionMetaDataMenu() {
        var _this = this;
        this._sliderDate = new SliderDate(this.SetJsonMetaData);
        this._numberAccuracy = new NumberAccuracy();

        var jsonMetaData = this.GetJsonMetaData();
        if (jsonMetaData != null) {
            var solutionMetaData = jsonMetaData;
            if (solutionMetaData.IsDate)
                this.SelectDate();
            else if (solutionMetaData.IsNumber)
                this.SelectNumber();
            else if (solutionMetaData.IsText)
                this.SelectText();
        } else {
            this.SelectText();
        }

        $("#Answer").keyup(function () {
            console.log(_this._current);
            if (_this._current.IsDate) {
                _this._sliderDate.SetDateUi();
            }
        });

        $("#btnMenuItemText").click(function () {
            _this.SelectText();
            $("#divMenuItemText").show();
        });
        $("#btnMenuItemText, #divMenuItemText").hover(function () {
            if (_this._current.IsText)
                $("#divMenuItemText").show();
        }, function () {
            $("#divMenuItemText").hide();
        });

        $("#btnMenuItemNumber").click(function () {
            _this.SelectNumber();
            $("#divMenuItemNumber").show();
        });
        $("#btnMenuItemNumber, #divMenuItemNumber").hover(function () {
            if (_this._current.IsNumber)
                $("#divMenuItemNumber").show();
        }, function () {
            $("#divMenuItemNumber").hide();
        });

        $("#btnMenuItemDate").click(function () {
            _this.SelectDate();
            $("#divMenuItemDate").show();
        });
        $("#btnMenuItemDate, #divMenuItemDate").hover(function () {
            if (_this._current.IsDate)
                $("#divMenuItemDate").show();
        }, function () {
            $("#divMenuItemDate").hide();
        });
    }
    SolutionMetaDataMenu.prototype.SelectText = function () {
        this.ResetAll();
        $("#infoMetaText").show();
        $("#btnMenuItemText").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataText());
    };

    SolutionMetaDataMenu.prototype.SelectNumber = function () {
        this.ResetAll();
        $("#infoMetaNumber").show();
        $("#btnMenuItemNumber").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataNumber());
    };

    SolutionMetaDataMenu.prototype.SelectDate = function () {
        this.ResetAll();
        $("#infoMetaDate").show();
        $("#btnMenuItemDate").addClass("active");

        var metaData = this.GetJsonMetaData();
        if (metaData != null && metaData.IsDate) {
            this._sliderDate.Set(metaData);
        }

        this.SetJsonMetaData(this._sliderDate.MetaData);
    };

    SolutionMetaDataMenu.prototype.ResetAll = function () {
        $("#infoMetaDate").hide();
        $("#infoMetaText").hide();
        $("#infoMetaNumber").hide();

        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").removeClass("active");
    };

    SolutionMetaDataMenu.prototype.GetJsonMetaData = function () {
        var jsonVal = $("#MetadataSolutionJson").val();
        if (jsonVal.length == 0)
            return null;

        return jQuery.parseJSON(jsonVal);
    };

    SolutionMetaDataMenu.prototype.SetJsonMetaData = function (json) {
        this._current = json;
        $("#MetadataSolutionJson").val(JSON.stringify(json));
    };
    return SolutionMetaDataMenu;
})();

var SliderDate = (function () {
    function SliderDate(SaveJson) {
        this.MetaData = new SolutionMetadataDate();
        this.SaveJson = SaveJson;
        var _this = this;
        this._slider = $("#sliderDate").slider({
            range: "min",
            value: 3,
            min: 1,
            max: 6,
            slide: function (event, ui) {
                _this.SetUiSlider(ui.value);
            },
            change: function (event, ui) {
                _this.SetUiSlider(ui.value);
            }
        });
    }
    SliderDate.prototype.Set = function (metaData) {
        this.MetaData = metaData;
        this._slider.slider("value", this.MetaData.Precision);
        this.SetDateUi();
    };

    SliderDate.prototype.SetUiSlider = function (sliderValue) {
        if (sliderValue == 1)
            this.MetaData.Precision = 1 /* Day */;
        else if (sliderValue == 2)
            this.MetaData.Precision = 2 /* Month */;
        else if (sliderValue == 3)
            this.MetaData.Precision = 3 /* Year */;
        else if (sliderValue == 4)
            this.MetaData.Precision = 4 /* Decade */;
        else if (sliderValue == 5)
            this.MetaData.Precision = 5 /* Century */;
        else if (sliderValue == 6)
            this.MetaData.Precision = 6 /* Millenium */;

        this.SetDateUi();
    };

    SliderDate.prototype.SetDateUi = function () {
        var text = SolutionMetadataDate.GetPrecisionLabel(this.MetaData.Precision);

        var dateR = DateParser.Run($("#Answer").val());

        if (!dateR.IsValid) {
            $("#spanEntryPrecision").html("keine g&#252;ltige Eingabe");
            $("#iDateError").show();
            $("#iDateCorrect").hide();
        } else {
            $("#spanEntryPrecision").html("<b>" + SolutionMetadataDate.GetPrecisionLabel(dateR.Precision) + "genau </b>" + "(" + dateR.ToLabel() + ")");
            $("#iDateError").hide();
            $("#iDateCorrect").show();
        }

        this.SaveJson(this.MetaData);
        $("#spanAnswerPrecision").text(text);
        $("#spanSliderValue").text(text);
    };
    return SliderDate;
})();

var NumberAccuracy = (function () {
    function NumberAccuracy() {
        $("#numberAccuracy").change(function () {
            var newVal = $(this).val().replace(/[^0-9]/g, '');
            $(this).val(newVal);
        });
    }
    return NumberAccuracy;
})();

var solutionMetaData = new SolutionMetaDataMenu();

$('.show-tooltip').tooltip();

$('#help').click(function () {
    $("#modalHelpSolutionType").modal('show');
});
//# sourceMappingURL=SolutionTypeText.js.map
