/// <reference path="../../Scripts/jquery.d.ts" />

class QuestionRow 
{
    Row: JQuery;
    QuestionId: number;
     
    constructor (divRow: JQuery) {
        this.Row = divRow;
        this.QuestionId = ~this.Row.attr("data-questionId");
	}

    SetCssClassSelected() { 
        this.Row.addClass("selected-row");
    }

    RemoveCssClassSelected() { 
        this.Row.removeClass("selected-row");
    }

    GetCheckbox() { 
        return new Checkbox($(this.Row.find(".selectQuestion")))
    }
}

class Checkbox extends QuestionRow
{
    CkbContainer: JQuery;
    private _ckb;

    constructor(ckbContainer) { 
        this.CkbContainer = ckbContainer; 	    
        this._ckb = $(this.CkbContainer.children()[0]);
		super(ckbContainer.closest(".question-row"));
    }
 	
    IsChecked() {  return this._ckb.is(':checked') }
    
    Check() 
    {  
        this._ckb.attr("checked", true); 
        this.SetCssClassSelected()
    }

    Uncheck() 
    {  
        this._ckb.attr("checked", false); 
        this.RemoveCssClassSelected()
    }
}

class RowSelector{
    
    selectedQuestions = new Array();

    Count() { 
        return this.selectedQuestions.length;
    }

    Toggle(ckb: Checkbox) {
        if (ckb.IsChecked()) {
            this.selectedQuestions.push(ckb)
            ckb.SetCssClassSelected();
        } else {
            this.selectedQuestions = jQuery.grep(this.selectedQuestions, function (value) {
                return value.QuestionId != ckb.QuestionId;
            });
            ckb.RemoveCssClassSelected();
        }

        this.UpdateToolbar();
    }

    UpdateToolbar() {
        if (this.Count() > 0) {
            $("#selectionCount").html("(" + this.Count() + ")");
        } else { 
            $("#selectionCount").html("");
        }
    }

    SelectAll() {
        this.selectedQuestions = new Array();
        var selectedQuestions = this.selectedQuestions
        
        $(".question-row").each(function () {
            var questionRow = new QuestionRow($(this));
            selectedQuestions.push(questionRow);
            questionRow.GetCheckbox().Check();
        });
        this.UpdateToolbar()
    }

    DeselecttAll() {

        $(".question-row").each(function () {
            new QuestionRow($(this)).GetCheckbox().Uncheck();
        });
        this.selectedQuestions = new Array();
        this.UpdateToolbar()
    }
}

$(function () {
    
    var rs = new RowSelector();
    
    $('.selectQuestion').change(function(){rs.Toggle(new Checkbox($(this)));});
    $('#selectAll').click(function () { rs.SelectAll(); });
    $('#selectNone').click(function () { rs.DeselecttAll(); });
    $('#selectMemorizedByMe').click(function () { });
    $('#selectCreatedByMe').click(function () { });
    $('#selectedNotMemorizedByMe').click(function () { });
    $('#selectNotCraetedByMe').click(function () { });
});
