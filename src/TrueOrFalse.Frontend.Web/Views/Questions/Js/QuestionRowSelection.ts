/// <reference path="Page.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />

class QuestionRow 
{
    Row: JQuery;
    QuestionId: number;
     
    constructor (divRow: JQuery) {
        this.Row = divRow;
        this.QuestionId = parseInt(this.Row.attr("data-questionId"));
	}

    SetCssClassSelected() { 
        this.Row.addClass("selected-row");
    }

    RemoveCssClassSelected() { 
        this.Row.removeClass("selected-row");
    }

    IsUserOwner() { 
        return this.Row.attr("data-userIsOwner") === "true";
    }

    IsMemorizedByUser() {
        return this.Row.find(".iAdded:visible").length > 0;
    }
 	
    IsChecked() { return this.Row.hasClass("selected-row"); }
    
    Check() 
    {  
        this.Row.addClass("selected-row");
        this.Row.find('.CheckboxText').html('Auswahl entfernen');
    }

    Uncheck() 
    {  
        this.Row.removeClass("selected-row");
        this.Row.find('.CheckboxText').html('Auswählen');
    }
}

class RowSelector{
    
    Rows = new Array<QuestionRow>();

    Count() { 
        return this.Rows.length;
    }

    Toggle(row: QuestionRow) {
        if (!row.IsChecked()) {
            this.Rows.push(row);
            row.Check();
        } else {
            this.Rows = jQuery.grep(this.Rows, function (value: QuestionRow) {
                return value.QuestionId != row.QuestionId;
            });
            row.Uncheck();
        }

        this.UpdateToolbar();
    }

    UpdateToolbar() {
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
    }

    SelectionContainsWhereIAmOwner() {
        for (var i = 0; i < this.Rows.length; i++) { 
            if((<QuestionRow>this.Rows[i]).IsUserOwner())
                return true;
        }
        return false;
    }

    SelectAll() {
        this.Rows = new Array();
        var rows = this.Rows;
        
        $(".question-row").each(function () {
            var questionRow = new QuestionRow($(this));
            rows.push(questionRow);
            questionRow.Check();
        });
        this.UpdateToolbar();
    }

    DeselecttAll() {
        $(".question-row").each(function () {
            new QuestionRow($(this)).Uncheck();
        });
        this.Rows = new Array();
        this.UpdateToolbar();
    }
    
    SelectAllWhereIAmOwner() { 
        this.SelectWhereConditionApplies((questionRow: QuestionRow) =>
            (!questionRow.IsChecked() && questionRow.IsUserOwner()));
    }

    SelectAllWhereIAmNotOwner() { 
        this.SelectWhereConditionApplies((questionRow: QuestionRow) =>
            (!questionRow.IsChecked() && !questionRow.IsUserOwner()));
    }

    SelectAllNotMemorizedByMe() {
        this.SelectWhereConditionApplies((questionRow: QuestionRow) =>
            (!questionRow.IsChecked() && !questionRow.IsMemorizedByUser()));
    }

    SelectAllMemorizedByMe() {
        this.SelectWhereConditionApplies((questionRow: QuestionRow) =>
            (!questionRow.IsChecked() && questionRow.IsMemorizedByUser()));
    }

    SelectWhereConditionApplies(conditionApplies : Function) {
        var rows = this.Rows;

        $(".question-row").each(function () {
            var questionRow = new QuestionRow($(this));

            if (conditionApplies(questionRow)) {
                questionRow.Check();
                rows.push(questionRow);
            }
        });

        this.UpdateToolbar();        
    }

    private IsSelected(row : QuestionRow) { 
        for (var i = 0; i < this.Rows.length; i++) { 
            if((<QuestionRow>this.Rows[i]).QuestionId == row.QuestionId)
                return true;
        }
        return false;
    }
}