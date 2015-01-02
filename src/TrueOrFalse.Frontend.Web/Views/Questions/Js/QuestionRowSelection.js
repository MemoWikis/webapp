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
        return this.Row.attr("data-userIsOwner") === "true";
    };

    QuestionRow.prototype.IsMemorizedByUser = function () {
        return this.Row.find(".iAdded:visible").length > 0;
    };

    QuestionRow.prototype.IsChecked = function () {
        return this.Row.hasClass("selected-row");
    };

    QuestionRow.prototype.Check = function () {
        this.Row.addClass("selected-row");
        this.Row.find('.CheckboxText').html('Auswahl entfernen');
    };

    QuestionRow.prototype.Uncheck = function () {
        this.Row.removeClass("selected-row");
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
            questionRow.Check();
        });
        this.UpdateToolbar();
    };

    RowSelector.prototype.DeselecttAll = function () {
        $(".question-row").each(function () {
            new QuestionRow($(this)).Uncheck();
        });
        this.Rows = new Array();
        this.UpdateToolbar();
    };

    RowSelector.prototype.SelectAllWhereIAmOwner = function () {
        this.SelectWhereConditionApplies(function (questionRow) {
            return (!questionRow.IsChecked() && questionRow.IsUserOwner());
        });
    };

    RowSelector.prototype.SelectAllWhereIAmNotOwner = function () {
        this.SelectWhereConditionApplies(function (questionRow) {
            return (!questionRow.IsChecked() && !questionRow.IsUserOwner());
        });
    };

    RowSelector.prototype.SelectAllNotMemorizedByMe = function () {
        this.SelectWhereConditionApplies(function (questionRow) {
            return (!questionRow.IsChecked() && !questionRow.IsMemorizedByUser());
        });
    };

    RowSelector.prototype.SelectAllMemorizedByMe = function () {
        this.SelectWhereConditionApplies(function (questionRow) {
            return (!questionRow.IsChecked() && questionRow.IsMemorizedByUser());
        });
    };

    RowSelector.prototype.SelectWhereConditionApplies = function (conditionApplies) {
        var rows = this.Rows;

        $(".question-row").each(function () {
            var questionRow = new QuestionRow($(this));

            if (conditionApplies(questionRow)) {
                questionRow.Check();
                rows.push(questionRow);
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
