/// <reference path="Page.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/underscore-typed.d.ts" />

class ToQuestionSetModal {

    Sets: QuestionSet[];

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

    Populate() { 
        var setResult = GetQuestionSetsForUser.Run();
        this.Sets = setResult.Sets;
        if (setResult.TotalSets == 0) {
            $("#tqsBody").hide();
            $("#tqsFooter").hide();
            $("#tqsNoSetsBody").show(200);
            $("#tqsNoSetsFooter").show(200);
        } else {
            $("#tqsNoSetsBody").hide();
            $("#tqsNoSetsFooter").hide();
            $("#tqsBody").show(200);
            $("#tqsFooter").show(200);

            var template = $("#tsqRowTemplate");

            $("[data-questionSetId]").remove();

            for (var i = 0; i < setResult.Sets.length; i++) {
                var newRow = template.clone().removeAttr("id").removeClass("hide");
                newRow.attr("data-questionSetId", setResult.Sets[i].Id);
                newRow.html(newRow.html().replace("{Name}", setResult.Sets[i].Name));
                var outerScope = this;
                newRow.click(function () { outerScope.SelectQuestion($(this)) });
                $("#tsqRowContainer").append(newRow);
            }
        }
    }

    SelectQuestion(questionSetRow: JQuery) {
        var id = questionSetRow.attr("data-questionSetId");

        this.Sets.map
        alert(id.toString())
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

class GetQuestionSetsForUserResult {
    TotalSets = 0;
    CurrentPage = 1;
    Sets: QuestionSet[] = new QuestionSet[];
}

class GetQuestionSetsForUser {
    static Run() {
        var result = new GetQuestionSetsForUserResult();

        $.ajax({
            type: 'POST', async: false, cache: false,
            url: "/Questions/GetQuestionSets/",
            error: function (error) { console.log(error) },
            success: function (r) {
                for (var i = 0; i < r.Sets.length; i++) { 
                    console.log(r.Sets[i])
                    result.TotalSets = r.Total;
                    result.CurrentPage = r.Total;
                    result.Sets.push(
                        new QuestionSet(
                            r.Sets[i].Id,
                            r.Sets[i].Name))
                }
            }
        });
        return result;
    }
}