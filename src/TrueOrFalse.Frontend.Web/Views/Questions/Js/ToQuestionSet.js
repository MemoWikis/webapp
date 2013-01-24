var ToQuestionSetModal = (function () {
    function ToQuestionSetModal() {
        $('#btnSelectionToSet').click(function () {
            _page.ToQuestionSetModal.Show();
        });
        this.Populate();
    }
    ToQuestionSetModal.prototype.Show = function () {
        this.Populate();
        $('#modalToQuestionSet').modal('show');
    };
    ToQuestionSetModal.prototype.Populate = function () {
        var setResult = GetQuestionSetsForUser.Run();
        this.Sets = setResult.Sets;
        if(setResult.TotalSets == 0) {
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
            for(var i = 0; i < setResult.Sets.length; i++) {
                var newRow = template.clone().removeAttr("id").removeClass("hide");
                newRow.attr("data-questionSetId", setResult.Sets[i].Id);
                newRow.html(newRow.html().replace("{Name}", setResult.Sets[i].Name));
                var outerScope = this;
                newRow.click(function () {
                    outerScope.SelectQuestion($(this));
                });
                $("#tsqRowContainer").append(newRow);
            }
        }
    };
    ToQuestionSetModal.prototype.SelectQuestion = function (questionSetRow) {
        var id = questionSetRow.attr("data-questionSetId");
        this.Sets.map;
        alert(id.toString());
    };
    return ToQuestionSetModal;
})();
var QuestionSet = (function () {
    function QuestionSet(Id, Name) {
        this.Id = Id;
        this.Name = Name;
    }
    return QuestionSet;
})();
var GetQuestionSetsForUserResult = (function () {
    function GetQuestionSetsForUserResult() {
        this.TotalSets = 0;
        this.CurrentPage = 1;
        this.Sets = new Array();
    }
    return GetQuestionSetsForUserResult;
})();
var GetQuestionSetsForUser = (function () {
    function GetQuestionSetsForUser() { }
    GetQuestionSetsForUser.Run = function Run() {
        var result = new GetQuestionSetsForUserResult();
        $.ajax({
            type: 'POST',
            async: false,
            cache: false,
            url: "/Questions/GetQuestionSets/",
            error: function (error) {
                console.log(error);
            },
            success: function (r) {
                for(var i = 0; i < r.Sets.length; i++) {
                    console.log(r.Sets[i]);
                    result.TotalSets = r.Total;
                    result.CurrentPage = r.Total;
                    result.Sets.push(new QuestionSet(r.Sets[i].Id, r.Sets[i].Name));
                }
            }
        });
        return result;
    }
    return GetQuestionSetsForUser;
})();
