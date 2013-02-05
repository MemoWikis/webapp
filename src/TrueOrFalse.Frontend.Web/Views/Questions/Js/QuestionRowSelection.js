var __extends = this.__extends || function (d, b) {
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var QuestionRow = (function () {
    function QuestionRow(divRow) {
        this.Row = divRow;
        this.QuestionId = parseInt(this.Row.attr("data-questionId"));
    }
    QuestionRow.prototype.SetCssClassSelected = function () {
        this.Row.addClass("selected-row");
    };
    QuestionRow.prototype.RemoveCssClassSelected = function () {
        this.Row.removeClass("selected-row");
    };
    QuestionRow.prototype.GetCheckbox = function () {
        return new Checkbox($(this.Row.find(".selectQuestion")));
    };
    QuestionRow.prototype.IsUserOwner = function () {
        return this.Row.attr("data-userIsOwner") == "true";
    };
    QuestionRow.prototype.IsMemorizedByUser = function () {
        return !(this.Row.find(".sliderValue").text() == "-1");
    };
    return QuestionRow;
})();
var Checkbox = (function (_super) {
    __extends(Checkbox, _super);
    function Checkbox(ckbContainer) {
        this.CkbContainer = ckbContainer;
        this._ckb = $(this.CkbContainer.children()[0]);
        _super.call(this, ckbContainer.closest(".question-row"));
    }
    Checkbox.prototype.IsChecked = function () {
        return this._ckb.is(':checked');
    };
    Checkbox.prototype.Check = function () {
        this._ckb.attr("checked", true);
        this.SetCssClassSelected();
    };
    Checkbox.prototype.Uncheck = function () {
        this._ckb.attr("checked", false);
        this.RemoveCssClassSelected();
    };
    return Checkbox;
})(QuestionRow);
var RowSelector = (function () {
    function RowSelector() {
        this.Rows = new Array();
    }
    RowSelector.prototype.Count = function () {
        return this.Rows.length;
    };
    RowSelector.prototype.Toggle = function (ckb) {
        if(ckb.IsChecked()) {
            this.Rows.push(ckb);
            ckb.SetCssClassSelected();
        } else {
            this.Rows = jQuery.grep(this.Rows, function (value) {
                return value.QuestionId != ckb.QuestionId;
            });
            ckb.RemoveCssClassSelected();
        }
        this.UpdateToolbar();
    };
    RowSelector.prototype.UpdateToolbar = function () {
        if(this.Count() > 0) {
            $("#selectionCount").html("(" + this.Count() + ")");
            $("#btnSelectionToSet").show();
            if(this.SelectionContainsWhereIAmOwner()) {
                $("#btnSelectionDelete").show();
            }
        } else {
            $("#selectionCount").html("");
            $("#btnSelectionToSet").hide();
            $("#btnSelectionDelete").hide();
        }
    };
    RowSelector.prototype.SelectionContainsWhereIAmOwner = function () {
        for(var i = 0; i < this.Rows.length; i++) {
            if((this.Rows[i]).IsUserOwner()) {
                return true;
            }
        }
        return false;
    };
    RowSelector.prototype.SelectAll = function () {
        this.Rows = new Array();
        var rows = this.Rows;
        $(".question-row").each(function () {
            var questionRow = new QuestionRow($(this));
            rows.push(questionRow);
            questionRow.GetCheckbox().Check();
        });
        this.UpdateToolbar();
    };
    RowSelector.prototype.DeselecttAll = function () {
        $(".question-row").each(function () {
            new QuestionRow($(this)).GetCheckbox().Uncheck();
        });
        this.Rows = new Array();
        this.UpdateToolbar();
    };
    RowSelector.prototype.SelectAllWhereIAmOwner = function () {
        var rows = this.Rows;
        $(".question-row").each(function () {
            var checkbox = new QuestionRow($(this)).GetCheckbox();
            if(!checkbox.IsChecked() && checkbox.IsUserOwner()) {
                checkbox.Check();
                rows.push(checkbox);
            }
        });
        this.UpdateToolbar();
    };
    RowSelector.prototype.SelectAllWhereIAmNotOwner = function () {
        var rows = this.Rows;
        $(".question-row").each(function () {
            var checkbox = new QuestionRow($(this)).GetCheckbox();
            if(!checkbox.IsChecked() && !checkbox.IsUserOwner()) {
                checkbox.Check();
                rows.push(checkbox);
            }
        });
        this.UpdateToolbar();
    };
    RowSelector.prototype.SelectAllMemorizedByMe = function () {
        var rows = this.Rows;
        $(".question-row").each(function () {
            var checkbox = new QuestionRow($(this)).GetCheckbox();
            console.log(checkbox.IsMemorizedByUser());
            if(!checkbox.IsChecked() && checkbox.IsMemorizedByUser()) {
                checkbox.Check();
                rows.push(checkbox);
            }
        });
        this.UpdateToolbar();
    };
    RowSelector.prototype.IsSelected = function (row) {
        for(var i = 0; i < this.Rows.length; i++) {
            if((this.Rows[i]).QuestionId == row.QuestionId) {
                return true;
            }
        }
        return false;
    };
    return RowSelector;
})();
$(function () {
    $('.selectQuestion').change(function () {
        _page.RowSelector.Toggle(new Checkbox($(this)));
    });
    $('#selectAll').click(function () {
        _page.RowSelector.SelectAll();
    });
    $('#selectNone').click(function () {
        _page.RowSelector.DeselecttAll();
    });
    $('#selectMemorizedByMe').click(function () {
        _page.RowSelector.SelectAllMemorizedByMe();
    });
    $('#selectCreatedByMe').click(function () {
        _page.RowSelector.SelectAllWhereIAmOwner();
    });
    $('#selectedNotMemorizedByMe').click(function () {
    });
    $('#selectNotCraetedByMe').click(function () {
        _page.RowSelector.SelectAllWhereIAmNotOwner();
    });
});
