/// <reference path="Page.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/underscore.d.ts" />

class ToQuestionSetModal {

    Sets: QuestionSet[];

    constructor() { 
        $('#btnSelectionToSet').click(function () {_page.ToQuestionSetModal.Show();});
    }

    Show() {
        this.Populate();
        $('#modalToQuestionSet').modal('show'); 
    }

    Populate() { 
        $('#tqsTitle').html(_page.RowSelector.Rows.length + " Fragen zu Fragesatz hinzufügen");
        
        var setResult = GetSetsForUser.Run();
        this.Sets = setResult.Sets;
            
        $("#tqsSuccess").hide();
        $("#tqsSuccessFooter").hide();
        if (setResult.TotalSets == 0) {
            $("#tqsBody").hide();
            $("#tqsNoSetsBody").show(200);
            $("#tqsNoSetsFooter").show(200);
        } else {
            $("#tqsNoSetsBody").hide();
            $("#tqsNoSetsFooter").hide();
            $("#tqsBody").show(200);
            $("#tqsTextSelectSet").show();

            var template = $("#tsqRowTemplate");

            $("[data-questionSetId]").remove();

            for (var i = 0; i < setResult.Sets.length; i++) {
                var newRow = template.clone().removeAttr("id").removeClass("hide2");
                newRow.attr("data-questionSetId", setResult.Sets[i].Id);
                newRow.html(newRow.html().replace("{Name}", setResult.Sets[i].Name));
                newRow.click(function () { _page.ToQuestionSetModal.AddToSet($(this)); });
                $("#tsqRowContainer").append(newRow);
            }
        }
    }

    AddToSet(questionSetRow: JQuery) {
        var id = parseInt(questionSetRow.attr("data-questionSetId"));

        var questionSet = _.filter(this.Sets, 
            function(pSet) { return pSet.Id == id; });

        $("#tqsSuccessMsg").html($("#tqsSuccessMsg").html()
            .replace('{Amount}', _page.RowSelector.Rows.length.toString())
            .replace('{SetName}', questionSet[0].Name)
            );

        SendQuestionsToAdd.Run(id);

        $("#tqsSuccess").show();
        $("#tqsSuccessFooter").show();
        $("#tqsBody").hide();
    }
}

class QuestionSet { 
    constructor(Id: number, Name: string) {
        this.Id = Id; 
        this.Name = Name;
    }
    Id : number;
    Name : string;
}

class GetSetsForUserResult {
    TotalSets = 0;
    CurrentPage = 1;
    Sets: Array<QuestionSet> = new Array<QuestionSet>();
}

class GetSetsForUser {
    static Run() {
        var result = new GetSetsForUserResult();

        $.ajax({
            type: 'POST', async: false, cache: false,
            url: "/Questions/GetQuestionSets/",
            error: function (error) { console.log(error); },
            success: function (r) {
                for (var i = 0; i < r.Sets.length; i++) { 
                    result.TotalSets = r.Total;
                    result.CurrentPage = r.Total;
                    result.Sets.push(
                        new QuestionSet(
                            r.Sets[i].Id,
                            r.Sets[i].Name));
                }
            }
        });
        return result;
    }
}

class SendQuestionsToAddResult {
    TotalSets = 0;
    CurrentPage = 1;
    Sets: Array<QuestionSet> = new Array<QuestionSet>();
}

class SendQuestionsToAdd { 
    static Run(questionSetId) { 

        var questionIds = _.reduce(_page.RowSelector.Rows, function (aggr : string, a) { 
            if (aggr.length == 0)
                return a.QuestionId;

            return aggr + "," + a.QuestionId; }, ""
        );

        $.ajax({
            type: 'POST', async: false, cache: false,
            url: "/Questions/AddToQuestionSet/",
            data: questionIds + ":" + questionSetId,
            error: function (error) { console.log(error); },
            success: function (result) { console.log(result); }
        });

        return new SendQuestionsToAddResult();
    }
}