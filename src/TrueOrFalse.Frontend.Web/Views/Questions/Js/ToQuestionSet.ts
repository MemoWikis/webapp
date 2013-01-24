/// <reference path="Page.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />


class ToQuestionSetModal 
{
    constructor() { 
        $('#btnSelectionToSet').click(function () {
            _page.ToQuestionSetModal.Show();
        });
        
        this.Populate();
    }

    Show() {
        this.Populate();
        $('#modalToQuestionSet').modal('show'); 
    }

    Populate() 
    { 
        if (_page.RowSelector.Count() == 0) {

        } else { 
        }
    }

}
