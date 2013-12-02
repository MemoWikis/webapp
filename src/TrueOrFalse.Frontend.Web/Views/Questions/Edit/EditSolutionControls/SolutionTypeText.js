var SolutionMetaDataMenu = (function () {
    function SolutionMetaDataMenu() {
        this._sliderDate = new SliderDate();
        this._numberAccuracy = new NumberAccuracy();
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
        var obj = jQuery.parseJSON('{"Name":"John"}');
        console.log(obj.Name);
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
            max: 2,
            value: 0,
            slide: function (event, ui) {
                _this.SetUiSlider(ui.value);
            },
            change: function (event, ui) {
                _this.SetUiSlider(ui.value);
            }
        });
    }
    SliderDate.prototype.SetUiSlider = function (sliderValue) {
        var text = "";
        if(sliderValue == 0) {
            text = "Tag";
        } else if(sliderValue == 1) {
            text = "Monat";
        } else if(sliderValue == 2) {
            text = "Jahr";
        }
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
$('#help').click(function () {
    $("#modalHelpSolutionType").modal('show');
});
