/// <reference path="Page.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
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

    QuestionRow.prototype.IsUserOwner = function () {
        return this.Row.attr("data-userIsOwner") == "true";
    };

    QuestionRow.prototype.IsMemorizedByUser = function () {
        return !(this.Row.find(".sliderValue").text() == "-1");
    };

    QuestionRow.prototype.IsChecked = function () {
        return this.Row.hasClass("selected-row");
    };

    QuestionRow.prototype.Check = function () {
        this.Row.addClass("selected-row");

        //this.Row.find('.CheckboxIcon').removeClass('fa-square-o');
        //this.Row.find('.CheckboxIcon').addClass('fa-check-square-o');
        this.Row.find('.CheckboxText').html('Auswahl entfernen');
    };

    QuestionRow.prototype.Uncheck = function () {
        this.Row.removeClass("selected-row");

        //this.Row.find('.CheckboxIcon').removeClass('fa-check-square-o');
        //this.Row.find('.CheckboxIcon').addClass('fa-square-o');
        this.Row.find('.CheckboxText').html('Auswählen');
    };
    return QuestionRow;
})();

var RowSelector = (function () {
    function RowSelector() {
        this.Rows = new Array();
    }
    RowSelector.prototype.Count = function () {
        return this.Rows.length;
    };

    RowSelector.prototype.Toggle = function (row) {
        if (!row.IsChecked()) {
            this.Rows.push(row);
            row.Check();
        } else {
            this.Rows = jQuery.grep(this.Rows, function (value) {
                return value.QuestionId != row.QuestionId;
            });
            row.Uncheck();
        }

        this.UpdateToolbar();
    };

    RowSelector.prototype.UpdateToolbar = function () {
        if (this.Count() > 0) {
            $("#selectionCount").html("(" + this.Count() + ")");
            $("#btnSelectionToSet").show();
            if (this.SelectionContainsWhereIAmOwner()) {
                $("#btnSelectionDelete").show();
            }
        } else {
            $("#selectionCount").html("");
            $("#btnSelectionToSet").hide();
            $("#btnSelectionDelete").hide();
        }
    };

    RowSelector.prototype.SelectionContainsWhereIAmOwner = function () {
        for (var i = 0; i < this.Rows.length; i++) {
            if (this.Rows[i].IsUserOwner())
                return true;
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
            if (!checkbox.IsChecked() && checkbox.IsUserOwner()) {
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
            if (!checkbox.IsChecked() && !checkbox.IsUserOwner()) {
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

            window.console.log(checkbox.IsMemorizedByUser());

            if (!checkbox.IsChecked() && checkbox.IsMemorizedByUser()) {
                checkbox.Check();
                rows.push(checkbox);
            }
        });

        this.UpdateToolbar();
    };

    RowSelector.prototype.IsSelected = function (row) {
        for (var i = 0; i < this.Rows.length; i++) {
            if (this.Rows[i].QuestionId == row.QuestionId)
                return true;
        }
        return false;
    };
    return RowSelector;
})();
//# sourceMappingURL=QuestionRowSelection.js.map
