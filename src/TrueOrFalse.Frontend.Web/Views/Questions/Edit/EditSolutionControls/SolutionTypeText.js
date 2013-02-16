var SolutionMetaDataMenu = (function () {
    function SolutionMetaDataMenu() {
        this._sliderDate = new SliderDate();
        this._spinnerNumberAccuracy = new SpinnerNumberAccuracy();
        $("#btnMenuItemText").click(this.SelectText);
        $("#btnMenuItemText, #divMenuItemText").hover(function () {
            $("#divMenuItemText").show();
        }, function () {
            $("#divMenuItemText").hide();
        });
        $("#btnMenuItemNumber").click(this.SelectNumber);
        $("#btnMenuItemNumber, #divMenuItemNumber").hover(function () {
            $("#divMenuItemNumber").show();
        }, function () {
            $("#divMenuItemNumber").hide();
        });
        $("#btnMenuItemDate").click(this.SelectDate);
        $("#btnMenuItemDate, #divMenuItemDate").hover(function () {
            $("#divMenuItemDate").show();
        }, function () {
            $("#divMenuItemDate").hide();
        });
    }
    SolutionMetaDataMenu.prototype.SelectText = function () {
        $("#btnMenuItemText").addClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").removeClass("active");
    };
    SolutionMetaDataMenu.prototype.SelectNumber = function () {
        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").addClass("active");
        $("#btnMenuItemDate").removeClass("active");
    };
    SolutionMetaDataMenu.prototype.SelectDate = function () {
        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").addClass("active");
    };
    return SolutionMetaDataMenu;
})();
var SliderDate = (function () {
    function SliderDate() {
        var _this = this;
        $("#sliderDate").slider({
            range: "min",
            max: 5,
            value: 3,
            slide: function (event, ui) {
                _this.SetUiSlider(ui.value);
            },
            change: function (event, ui) {
                _this.SetUiSlider(ui.value);
            }
        });
    }
    SliderDate.prototype.SetUiSlider = function (sliderValue) {
        $("#spanSliderValue").text(sliderValue);
    };
    return SliderDate;
})();
var SpinnerNumberAccuracy = (function () {
    function SpinnerNumberAccuracy() {
        $("#numberAccuracy").spinner({
            min: 0,
            max: 100,
            step: 1,
            start: 0
        });
    }
    return SpinnerNumberAccuracy;
})();
var solutionMetaData = new SolutionMetaDataMenu();
$('#help').click(function () {
    $("#modalHelpSolutionType").modal('show');
});
