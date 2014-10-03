/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/utils.ts" />

var answerResult;

var answerHistory = [];
var amountOfTries = 0;

interface ISolutionEntry {
    GetAnswerText(): string;
    GetAnswerData(): {};
    OnNewAnswer(): void;
}

class AnswerQuestion
{
    private _getAnswerText: () => string;
    private _getAnswerData: () => {};
    private _onNewAnswer : () => void;

    constructor(solutionEntry : ISolutionEntry)
    {
        this._getAnswerText = solutionEntry.GetAnswerText;
        this._getAnswerData = solutionEntry.GetAnswerData;
        this._onNewAnswer = solutionEntry.OnNewAnswer;

        var self = this;

        $("#txtAnswer").keypress(function (e) {
            if (e.keyCode == 13) {
                if (self.isAnswerPossible()) {
                    self.validateAnswer();
                    return false;
                }
            }
            return true;
        });

        $("#btnCheck").click(
            e => {
                e.preventDefault();
                this.validateAnswer();
            });

        $("#btnCheckAgain").click(
            e=> {
                e.preventDefault();
                this.validateAnswer();
        });

        $("#errorTryCount").click(function () {
            var divAnswerHistory = $("#divAnswerHistory");
            if (!divAnswerHistory.is(":visible"))
                divAnswerHistory.show();
            else
                divAnswerHistory.hide();
        });

        $(".selectorShowAnswer").click( ()=> {
            InputFeedback.ShowCorrectAnswer(); return false;
        });
        $("#buttons-edit-answer").click((e) => {
            e.preventDefault();
            this._onNewAnswer();

            InputFeedback.AnimateNeutral();
        });
    }

    private validateAnswer() {
        var answerText = this._getAnswerText();

        amountOfTries++;
        answerHistory.push(answerText);

        if (answerText.trim().length == 0) {
            InputFeedback.ShowError("Du könntest es es ja wenigstens probieren! Tzzzz... ", true); return false;
        }

        $("#answerHistory").html("<i class='fa fa-spinner fa-spin' style=''></i>");

        $.ajax({
            type: 'POST',
            url: window.ajaxUrl_SendAnswer,
            data: this._getAnswerData(),
            cache: false,
            success: function(result) {
                answerResult = result;
                $("#buttons-first-try").hide();
                $("#buttons-answer-again").hide();
                if (result.correct) {
                    InputFeedback.ShowSuccess();
                } else {
                    InputFeedback.ShowError();
                };

                $("#answerHistory").empty();
                $.post("/AnswerQuestion/PartialAnswerHistory", { questionId: window.questionId}, function(data) {
                    $("#answerHistory").html(data);
                });
            }
        });
        return false;
    }

    private isAnswerPossible() {

        if ($("#buttons-first-try").is(":visible"))
            return true;

        if ($("#buttons-edit-answer").is(":visible"))
            return true;

        if ($("#buttons-answer-again").is(":visible"))
            return true;

        return false;
    }   

    public OnAnswerChange() {
        this.Reenable_answer_button_if_renewed_answer();
    }

    public Reenable_answer_button_if_renewed_answer() {
        if ($("#buttons-edit-answer").is(":visible")) {
            $("#buttons-edit-answer").hide();
            $("#buttons-answer-again").show();
            InputFeedback.AnimateNeutral();
        }
    }
}

class InputFeedback {

    private static ErrMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter nicht aufgeben.",
        "Übung macht den Meister, Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
    ];

    private static SuccessMsgs = ["Yeah! Weiter so.", "Du bis auf einem guten Weg.", "Sauber!", "Well Done!"];

    public static ShowError(text = "", forceShow: boolean = false)
    {
        if (text == "") {
            text = InputFeedback.ErrMsgs[Utils.Random(0, InputFeedback.ErrMsgs.length - 1)];
        }

        var errorTryText;
        var amountOfTriesText = ["ein Versuch", "zwei", "drei", "vier", "fünf", "sehr hartnäckig", "Respekt!"];

        switch (amountOfTries) {
            case 1:
                errorTryText = amountOfTriesText[amountOfTries - 1]; break;
            case 2:
            case 3:
            case 4:
            case 5:
                errorTryText = amountOfTriesText[amountOfTries - 1] + " Versuche"; break;
            case 6:
            case 7:
                errorTryText = amountOfTriesText[amountOfTries - 1]; break;
            default:
                errorTryText = amountOfTriesText[6];
        }
        $("#errorTryCount").html("(" + errorTryText + ")");

        $('#ulAnswerHistory').html("");
        $.each(answerHistory, function (index, val) {
            $('#ulAnswerHistory').append(
                $('<li>' + val + '</li>'));
        });

        $("#divWrongAnswer").show();
        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();

        if (forceShow || Utils.Random(1, 10) % 4 == 0) {
            $("#answerFeedback").html(text).show();
        } else {
            $("#answerFeedback").html(text).hide();
        }

        InputFeedback.AnimateWrongAnswer();
     }

    static AnimateWrongAnswer() {
        $("#buttons-edit-answer").show();
        $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
    }

    static AnimateNeutral() {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);
    }

    static ShowSuccess() {
        $("#buttons-next-answer").show();
        $("#buttons-edit-answer").hide();
        $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
        $("#divWrongAnswer").hide();

        $("#divAnsweredCorrect").show();
        $("#wellDoneMsg").html("" + InputFeedback.SuccessMsgs[Utils.Random(0, InputFeedback.SuccessMsgs.length - 1)]).show();
    }

    static ShowCorrectAnswer() {

        InputFeedback.ShowNextAnswer(); 
        $("#divWrongAnswer").hide();
        $("#SolutionDetails").show();

        ajaxGetAnswer(function (result) {
            $("#Solution .Content").html(result.correctAnswer);
            if (result.correctAnswerDesc) {
                $("#Description").show().find('.Content').html(result.correctAnswerDesc);
            }
            if (result.correctAnswerReferences.length != 0) {
                $("#References").show();
                for (var i = 0; i < result.correctAnswerReferences.length; i++) {
                    var reference = result.correctAnswerReferences[i];
                    var referenceHtml = $('<div class="ReferenceDetails"></div>');
                    referenceHtml.appendTo('#References .Content');

                    var fn = function (div, ref) {
                        if (ref.referenceText) {
                            if (ref.referenceType == 'UrlReference') {
                                $('<div class="ReferenceText"><a href="' + ref.referenceText + '">' + ref.referenceText + '</a></div>').appendTo(div);
                            } else
                                $('<div class="ReferenceText">' + ref.referenceText + '</div>').appendTo(div);
                        }
                        if (ref.additionalInfo) {
                            $('<div class="AdditionalInfo">' + ref.additionalInfo + '</div>').appendTo(div);
                        }
                    }

                    var fnAjaxCall = function (div, ref) {
                        $.ajax({
                            url: '/Fragen/Bearbeite/ReferencePartial?catId=' + ref.categoryId,
                            type: 'GET',
                            success: function (data) {
                                div.prepend(data);
                                fn(div, ref);

                                $('.show-tooltip').tooltip();
                            }
                        });
                    }

                    if (reference.categoryId != -1) {
                        fnAjaxCall(referenceHtml, reference);
                    } else {
                        fn(referenceHtml, reference);
                    }                    
                }
            }
        });
    }

    private static ShowNextAnswer() {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);

        $("#buttons-next-answer").show();

        $("#answerFeedback").hide();

        $("#buttons-first-try").hide();
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").hide();
    }

}

function ajaxGetAnswer(onSuccessAction) {
    $.ajax({
        type: 'POST',
        url: window.ajaxUrl_GetAnswer,
        cache: false,
        success: function (result) {
            onSuccessAction(result);
        }
    });
}


