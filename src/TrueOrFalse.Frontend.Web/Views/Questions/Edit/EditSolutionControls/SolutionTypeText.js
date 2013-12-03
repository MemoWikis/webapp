var SolutionMetaDataMenu = (function () {
    function SolutionMetaDataMenu() {
        var _this = this;
        this._sliderDate = new SliderDate();
        this._numberAccuracy = new NumberAccuracy();
        var jsonMetaData = this.GetJsonMetaData();
        if(jsonMetaData != null) {
            var solutionMetaData = jsonMetaData;
            if(solutionMetaData.IsDate) {
                this.SelectDate();
            } else if(solutionMetaData.IsNumber) {
                this.SelectNumber();
            } else if(solutionMetaData.IsText) {
                this.SelectText();
            }
        } else {
            this.SelectText();
        }
        $("#btnMenuItemText").click(function () {
            _this.SelectText();
            $("#divMenuItemText").show();
        });
        $("#btnMenuItemText, #divMenuItemText").hover(function () {
            if(_this._current.IsText) {
                $("#divMenuItemText").show();
            }
        }, function () {
            $("#divMenuItemText").hide();
        });
        $("#btnMenuItemNumber").click(function () {
            _this.SelectNumber();
            $("#divMenuItemNumber").show();
        });
        $("#btnMenuItemNumber, #divMenuItemNumber").hover(function () {
            if(_this._current.IsNumber) {
                $("#divMenuItemNumber").show();
            }
        }, function () {
            $("#divMenuItemNumber").hide();
        });
        $("#btnMenuItemDate").click(function () {
            _this.SelectDate();
            $("#divMenuItemDate").show();
        });
        $("#btnMenuItemDate, #divMenuItemDate").hover(function () {
            if(_this._current.IsDate) {
                $("#divMenuItemDate").show();
            }
        }, function () {
            $("#divMenuItemDate").hide();
        });
    }
    SolutionMetaDataMenu.prototype.SelectText = function () {
        this.ResetAll();
        $("#btnMenuItemText").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataText());
    };
    SolutionMetaDataMenu.prototype.SelectNumber = function () {
        this.ResetAll();
        $("#btnMenuItemNumber").addClass("active");
        this.SetJsonMetaData(new SolutionMetadataNumber());
    };
    SolutionMetaDataMenu.prototype.SelectDate = function () {
        this.ResetAll();
        $("#infoMetaDate").show();
        $("#btnMenuItemDate").addClass("active");
        var metaData = this.GetJsonMetaData();
        if(metaData != null && metaData.IsDate) {
            metaData = metaData;
            $("#sliderDate").val(metaData.Precision);
            this.SetJsonMetaData(metaData);
        } else {
            this.SetJsonMetaData(new SolutionMetadataDate());
        }
    };
    SolutionMetaDataMenu.prototype.ResetAll = function () {
        $("#infoMetaDate").hide();
        $("#btnMenuItemText").removeClass("active");
        $("#btnMenuItemNumber").removeClass("active");
        $("#btnMenuItemDate").removeClass("active");
    };
    SolutionMetaDataMenu.prototype.GetJsonMetaData = function () {
        var jsonVal = $("#MetadataSolutionJson").val();
        if(jsonVal.length == 0) {
            return null;
        }
        return jQuery.parseJSON(jsonVal);
    };
    SolutionMetaDataMenu.prototype.SetJsonMetaData = function (json) {
        this._current = json;
        $("#MetadataSolutionJson").val(JSON.stringify(json));
    };
    return SolutionMetaDataMenu;
})();
var SliderDate = (function () {
    function SliderDate() {
        var _this = this;
        $("#sliderDate").slider({
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
    SliderDate.prototype.SetUiSlider = function (sliderValue) {
        var text = "";
        if(sliderValue == 1) {
            text = "Tag";
        } else if(sliderValue == 2) {
            text = "Monat";
        } else if(sliderValue == 3) {
            text = "Jahr";
        } else if(sliderValue == 4) {
            text = "Jahrzent";
        } else if(sliderValue == 5) {
            text = "Jahrhundert";
        } else if(sliderValue == 6) {
            text = "Jahrtausend";
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
