/// <reference path="Page.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/underscore.d.ts" />

class ToQuestionSetModal {

    Sets: QuestionSet[];
    _template: JQuery;
    _templateSuccessMsg: string;

    constructor() {
        $('#btnSelectionToSet').click(function (e) { _page.ToQuestionSetModal.Show(e); });
        this._templateSuccessMsg = $("#tqsSuccessMsg").html();
    }

    Show(e) {
        e.preventDefault();
        if (NotLoggedIn.Yes()) {
            NotLoggedIn.ShowErrorMsg("AddQuestionToSet");
            return;
        }

        this.Populate();
        $('#modalToQuestionSet').modal('show');
    }

    Populate() {
        $("#tqsSuccess").hide();
        $("#tqsSuccessFooter").hide();
        var questionCount = _page.RowSelector.Rows.length;

        if (questionCount == 0) {
            $('#tqsTitle').html("Keine Fragen ausgewählt");
            $("#tqsBody").hide();
            $("#tqsFooter").hide();
            $("#tqsNoQuestionsSelectedBody").show(200);
            $("#tqsNoQuestionsSelectedFooter").show();
        } else {
            
            $('#tqsTitle').html(questionCount + " Fragen zu Lernset hinzuf&uuml;gen");

            var setResult = GetSetsForUser.Run($("#txtTqsSetFilter").val());
            this.Sets = setResult.Sets;
            this._template = $("#tqsRowTemplate");

            if (setResult.TotalSets == 0) {
                $("#tqsBody").hide();
                $("#tqsFooter").hide();
                $("#tqsNoQuestionsSelectedBody").hide();
                $("#tqsNoQuestionsSelectedFooter").hide();
                $("#tqsNoSetsBody").show(200);
                $("#tqsNoSetsFooter").show(200);
            } else {
                $("#tqsNoQuestionsSelectedBody").hide();
                $("#tqsNoQuestionsSelectedFooter").hide();
                $("#tqsNoSetsBody").hide();
                $("#tqsNoSetsFooter").hide();
                $("#tqsBody").show(200);
                $("#tqsFooter").show();
                //$("#tqsTextSelectSet").show();

                this.PopulateSets();
            }
        }

        $("#txtTqsSetFilter").keyup(() => {
            var result = GetSetsForUser.Run($("#txtTqsSetFilter").val());
            this.Sets = result.Sets;
            $("#tqsSetCount").html("Treffer " + result.TotalSets.toString());
            this.PopulateSets();
        });
    }

    AddToSet(questionSetRow: JQuery) {
        var id = parseInt(questionSetRow.attr("data-questionSetId"));

        var questionSet = _.filter(this.Sets,
            function (pSet : QuestionSet) { return pSet.Id == id; });

        var result = SendQuestionsToAdd.Run(id);

        var msgNonAdded = "";
        if (result.QuestionAlreadyInSet > 0)
            msgNonAdded = "<br/>" + result.QuestionAlreadyInSet + " Fragen waren bereits Teil des Lernsets.";

        $("#tqsSuccessMsg").html(this._templateSuccessMsg
            .replace('{Amount}', result.QuestionsAddedCount.toString())
            .replace('{SetName}', questionSet[0].Name)
            .replace('{NonAdded}', msgNonAdded)
            );

        $("#tqsSuccess").show();
        $("#tqsSuccessFooter").show();
        $("#tqsBody").hide();
        $("#tqsFooter").hide();
    }

    PopulateSets() {
        $("[data-questionSetId]").remove();

        for (var i = 0; i < this.Sets.length; i++) {
            var newRow = this._template.clone().removeAttr("id").removeClass("hide2");
            newRow.attr("data-questionSetId", this.Sets[i].Id);
            newRow.html(newRow.html()
                .replace("{Name}", this.Sets[i].Name)
                .replace("{AnzahlFragen}", this.Sets[i].QuestionCount.toString()));
            newRow.click(function () { _page.ToQuestionSetModal.AddToSet($(this)); });
            $("#tqsRowContainer").append(newRow);
        }
    }
}

class QuestionSet {
    constructor(Id: number, Name: string, QuestionCount: number) {
        this.Id = Id;
        this.Name = Name;
        this.QuestionCount = QuestionCount;
    }
    Id: number;
    Name: string;
    QuestionCount: number;
}

class GetSetsForUserResult {
    TotalSets = 0;
    CurrentPage = 1;
    Sets: Array<QuestionSet> = new Array<QuestionSet>();
}

class GetSetsForUser {
    static Run(filter: string) {
        var result = new GetSetsForUserResult();

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { filter: filter },
            url: "/Questions/GetQuestionSets/",
            error: function (error) { console.log(error); },
            success: function (r) {
                for (var i = 0; i < r.Sets.length; i++) {
                    result.TotalSets = r.Total;
                    result.Sets.push(
                        new QuestionSet(
                            r.Sets[i].Id,
                            r.Sets[i].Name,
                            r.Sets[i].QuestionCount));
                }
            }
        });
        return result;
    }
}

class SendQuestionsToAddResult {
    QuestionsAddedCount: number;
    QuestionAlreadyInSet: number;
}

class SendQuestionsToAdd {
    static Run(questionSetId) {

        var questionIds = _.reduce(_page.RowSelector.Rows, function (aggr: string, a : any) {
            if (aggr.length == 0)
                return a.QuestionId;

            return aggr + "," + a.QuestionId;
        }, ""
            );

        var result = new SendQuestionsToAddResult();
        $.ajax({
            type: 'POST', async: false, cache: false,
            url: "/Questions/AddToQuestionSet/",
            data: questionIds + ":" + questionSetId,
            error: function (error) { console.log(error); },
            success: function (data) {
                result.QuestionsAddedCount = data.QuestionsAddedCount;
                result.QuestionAlreadyInSet = data.QuestionAlreadyInSet;
            }
        });

        return result;
    }
}