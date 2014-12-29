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

    GetCheckbox() { 
        return new Checkbox($(this.Row.find(".selectQuestion")))
    }

    IsUserOwner() { 
        return this.Row.attr("data-userIsOwner") == "true";
    }

    IsMemorizedByUser() { 
        return !(this.Row.find(".sliderValue").text() == "-1")
    }
}

class Checkbox extends QuestionRow
{
    CkbContainer: JQuery;
    private _ckb;

    constructor(ckbContainer) { 
        this.CkbContainer = ckbContainer; 	    
        this._ckb = $(this.CkbContainer.find('input[type="checkbox"]'));
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
    
    Rows = new Array();

    Count() { 
        return this.Rows.length;
    }

    Toggle(ckb: Checkbox) {
        if (ckb.IsChecked()) {
            this.Rows.push(ckb)
            ckb.SetCssClassSelected();
        } else {
            this.Rows = jQuery.grep(this.Rows, function (value) {
                return value.QuestionId != ckb.QuestionId;
            });
            ckb.RemoveCssClassSelected();
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
        var rows = this.Rows
        
        $(".question-row").each(function () {
            var questionRow = new QuestionRow($(this));
            rows.push(questionRow);
            questionRow.GetCheckbox().Check();
        });
        this.UpdateToolbar()
    }

    DeselecttAll() {
        $(".question-row").each(function () {
            new QuestionRow($(this)).GetCheckbox().Uncheck();
        });
        this.Rows = new Array();
        this.UpdateToolbar()
    }
    
    SelectAllWhereIAmOwner() { 
        var rows = this.Rows;
        
        $(".question-row").each(function () {
            var checkbox = new QuestionRow($(this)).GetCheckbox();
            if (!checkbox.IsChecked() && checkbox.IsUserOwner()) { 
                checkbox.Check();
                rows.push(checkbox);
            }
        });

        this.UpdateToolbar()
    }

    SelectAllWhereIAmNotOwner() { 
        var rows = this.Rows;
        
        $(".question-row").each(function () {
            var checkbox = new QuestionRow($(this)).GetCheckbox();
            if (!checkbox.IsChecked() && !checkbox.IsUserOwner()) { 
                checkbox.Check();
                rows.push(checkbox);
            }
        });

        this.UpdateToolbar()
    }

    SelectAllMemorizedByMe() { 
        var rows = this.Rows;
        
        $(".question-row").each(function () {
            var checkbox = new QuestionRow($(this)).GetCheckbox();

            console.log(checkbox.IsMemorizedByUser());

            if (!checkbox.IsChecked() && checkbox.IsMemorizedByUser()) { 
                checkbox.Check();
                rows.push(checkbox);
            }
        });

        this.UpdateToolbar()
    }

    private IsSelected(row : QuestionRow) { 
        for (var i = 0; i < this.Rows.length; i++) { 
            if((<QuestionRow>this.Rows[i]).QuestionId == row.QuestionId)
                return true;
        }
        return false;
    }
}