var answerResult;

var choices = [];

class AnswerQuestion {
    private _getAnswerText: () => string;
    private _getAnswerData: () => {};
    private _onNewAnswer: () => void;

    private _onCorrectAnswer: () => void = () => {};
    private _onWrongAnswer: () => void = () => { };

    private _inputFeedback: AnswerQuestionUserFeedback;
    private _isLastLearningStep = false;

    static ajaxUrl_SendAnswer: string;
    static ajaxUrl_GetSolution: string;
    static ajaxUrl_CountLastAnswerAsCorrect: string;
    static ajaxUrl_CountUnansweredAsCorrect: string;
    static ajaxUrl_TestSessionRegisterAnsweredQuestion: string;
    static ajaxUrl_LearningSessionAmendAfterShowSolution: string;
    static TestSessionProgessAfterAnswering: number;
    static IsLastTestSessionStep = false;
    static TestSessionId: number;

    public IsGameMode: boolean;
    public IsLearningSession = false;
    public IsTestSession = false;

    public AnsweredCorrectly = false;
    public AnswersSoFar = [];
    public AmountOfTries = 0;
    public AtLeastOneWrongAnswer = false;
    public AnswerCountedAsCorrect = false;
    public ShowedSolutionOnly = false;

    constructor(answerEntry: IAnswerEntry) {

        this.IsGameMode = answerEntry.IsGameMode;
        if ($('#hddIsLearningSession').length === 1)
            this.IsLearningSession = $('#hddIsLearningSession').val().toLowerCase() === "true";

        if (this.IsLearningSession && $('#hddIsLearningSession').attr('data-is-last-step'))
            this._isLastLearningStep = $('#hddIsLearningSession').attr('data-is-last-step').toLowerCase() === "true";

        if ($('#hddIsTestSession').length === 1)
            this.IsTestSession = $('#hddIsTestSession').val().toLowerCase() === "true";

        if (this.IsTestSession && $('#hddIsTestSession').attr('data-is-last-step'))
            AnswerQuestion.IsLastTestSessionStep = $('#hddIsTestSession').attr('data-is-last-step').toLowerCase() === "true";

        if (this.IsTestSession && $('#hddIsTestSession').attr('data-test-session-id'))
            AnswerQuestion.TestSessionId = parseInt($('#hddIsTestSession').attr('data-test-session-id'));

        this._getAnswerText = answerEntry.GetAnswerText;
        this._getAnswerData = answerEntry.GetAnswerData;
        this._onNewAnswer = answerEntry.OnNewAnswer;

        AnswerQuestion.ajaxUrl_SendAnswer = $("#ajaxUrl_SendAnswer").val();
        AnswerQuestion.ajaxUrl_GetSolution = $("#ajaxUrl_GetSolution").val();
        AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect = $("#ajaxUrl_CountLastAnswerAsCorrect").val();
        AnswerQuestion.ajaxUrl_CountUnansweredAsCorrect = $("#ajaxUrl_CountUnansweredAsCorrect").val();
        AnswerQuestion.ajaxUrl_TestSessionRegisterAnsweredQuestion = $("#ajaxUrl_TestSessionRegisterAnsweredQuestion").val();
        AnswerQuestion.ajaxUrl_LearningSessionAmendAfterShowSolution = $("#ajaxUrl_LearningSessionAmendAfterShowSolution").val();
        AnswerQuestion.TestSessionProgessAfterAnswering = $("#TestSessionProgessAfterAnswering").val();

        this._inputFeedback = new AnswerQuestionUserFeedback(this);

        var self = this;

        $('body').keydown(function(e) {
            var target = $(e.target);
            if (e.keyCode == 13 && (target.parents("#AnswerBody").length)) {
                $("#btnCheck:visible").click();
                $("#btnEditAnswer:visible").click();
                $("#btnCheckAgain:visible").click();
                if ($('#btnNext').is(':visible')) {
                    window.location.href = $('#btnNext:visible').attr('href');
                }
            }
        });

        $("#txtAnswer")
            .keypress(e => {
                if (e.keyCode == 13) {
                    return false;
                }
                return true;
            });

        $("#btnCheck, #btnCheckAgain")
            .click(
                e => {
                    e.preventDefault();
                    $('#hddTimeRecords').attr('data-time-of-answer', $.now());
                });

        $("#btnCheck")
            .click(
            e => {
                    e.preventDefault();
                    self.ValidateAnswer();
                });

        $("#btnCheckAgain")
            .click(
                e => {
                    e.preventDefault();
                    self.ValidateAnswer();
                });

        $("#aCountAsCorrect")
            .click(
                e => {
                    e.preventDefault();
                    self.countAnswerAsCorrect();
                });

        $("#CountWrongAnswers")
            .click(e => {
                e.preventDefault();
                var divWrongAnswers = $("#divWrongAnswers");
                if (!divWrongAnswers.is(":visible"))
                    divWrongAnswers.show();
                else
                    divWrongAnswers.hide();
            });

        $(".selectorShowSolution")
            .click(() => {
                this._inputFeedback.ShowSolution();
                return false;
            });

        $("#buttons-edit-answer")
            .click((e) => {
                e.preventDefault();
                this._onNewAnswer();

                this._inputFeedback.AnimateNeutral();
            });

        $("#btnNext, #aSkipStep")
            .click(function(e) {
                if (self.IsLearningSession && self.AmountOfTries === 0 && !self.AnswerCountedAsCorrect && !self.ShowedSolutionOnly) {
                    var href = $(this).attr('href') +
                        "?skipStepIdx=" +
                        $('#hddIsLearningSession').attr('data-current-step-idx');
                    window.location.href = href;
                    return false;
                }
                return true;
            });
    }

    public OnCorrectAnswer(func: () => void) {
        this._onCorrectAnswer = func;
    }

    public OnWrongAnswer(func: () => void) {
        this._onWrongAnswer = func;
    }

    static GetQuestionId(): number {
        return +$("#questionId").val();
    }

    IsLastQuestion(): boolean {
        return $("#isLastQuestion").val() === "True";
    }

    private ValidateAnswer() {
        var answerText
            = this._getAnswerText();
        var self = this;

        if (answerText.trim().length === 0) {
            $('#spnWrongAnswer').hide();
            self._inputFeedback
                .ShowError("Du könntest es ja wenigstens probieren ... (Wird nicht als Antwortversuch gewertet.)",
                    true);
            return false;
        } else {
            $('#spnWrongAnswer').show();
            self.AmountOfTries++;
            self.AnswersSoFar.push(answerText);
            $("#progressPercentageDone").width(AnswerQuestion.TestSessionProgessAfterAnswering + "%");
            $("#spanPercentageDone").html(AnswerQuestion.TestSessionProgessAfterAnswering + "%");

            var test = $.extend(self._getAnswerData(),
            {
                questionViewGuid: $('#hddQuestionViewGuid').val(),
                interactionNumber: $('#hddInteractionNumber').val(),
                millisecondsSinceQuestionView: AnswerQuestion
                    .TimeSinceLoad($('#hddTimeRecords').attr('data-time-of-answer'))
            });

            $("#answerHistory").html("<i class='fa fa-spinner fa-spin' style=''></i>");
            $.ajax({
                type: 'POST',
                url: AnswerQuestion.ajaxUrl_SendAnswer,
                data: $.extend(self._getAnswerData(),
                {
                    questionViewGuid: $('#hddQuestionViewGuid').val(),
                    interactionNumber: $('#hddInteractionNumber').val(),
                    millisecondsSinceQuestionView: AnswerQuestion
                        .TimeSinceLoad($('#hddTimeRecords').attr('data-time-of-answer'))
                }),
                cache: false,
                success(result) {
                    answerResult = result;
                    self.IncrementInteractionNumber();

                    $("#buttons-first-try").hide();
                    $("#buttons-answer-again").hide();

                    if (self.IsTestSession) {
                        $.ajax({
                            type: 'POST',
                            url: AnswerQuestion.ajaxUrl_TestSessionRegisterAnsweredQuestion,
                            data: {
                                testSessionId: AnswerQuestion.TestSessionId,
                                questionId: AnswerQuestion.GetQuestionId(),
                                questionViewGuid: $('#hddQuestionViewGuid').val(),
                                answeredQuestion: true
                            },
                            cache: false
                        });
                        if (AnswerQuestion.IsLastTestSessionStep) {
                            $('#btnNext').html('Zum Ergebnis');
                        }
                    }

                    if (result.correct)
                        self.HandleCorrectAnswer();
                    else 
                        self.HandleWrongAnswer(result, answerText);

                    $("#answerHistory").empty();
                    $.post("/AnswerQuestion/PartialAnswerHistory",
                        { questionId: AnswerQuestion.GetQuestionId() },
                        function(data) {
                            $("#answerHistory").html(data);
                        });
                }
            });
            return false;
        }
    }

    private HandleCorrectAnswer() {
        this.AnsweredCorrectly = true;
        this._inputFeedback.ShowSuccess();
        this._inputFeedback.ShowSolution();
        if (this._isLastLearningStep)
            $('#btnNext').html('Zum Ergebnis');

        this._onCorrectAnswer();
    }

    private HandleWrongAnswer(result: any, answerText : string) {
        if (this._isLastLearningStep && !result.newStepAdded)
            $('#btnNext').html('Zum Ergebnis');

        if (this.IsGameMode) {
            this._inputFeedback.ShowErrorGame();
            this._inputFeedback.ShowSolution();
        } else {
            this._inputFeedback.UpdateAnswersSoFar();

            this.RegisterWrongAnswer();
            this._inputFeedback.ShowError();

            if (this.IsLearningSession || this.IsTestSession) {

                this._inputFeedback.ShowSolution();
                $('#CountWrongAnswers, #divWrongAnswers').hide();

            } else if (result.choices != null) { //if multiple choice
                choices = result.choices;
                if (this.allWrongAnswersTried(answerText)) {
                    this._inputFeedback.ShowSolution();
                }
            }
        }

        this._onWrongAnswer();
    }
    

    private RegisterWrongAnswer() {
        if (this.AtLeastOneWrongAnswer) return;
        this.AtLeastOneWrongAnswer = true;
        $('#aCountAsCorrect')
            .attr('data-original-title',
                'Drücke hier und deine letzte Antwort wird als richtig gewertet (bei anderer Schreibweise, Formulierung ect). Aber nicht schummeln!');
    }

    private allWrongAnswersTried(answerText: string) {
        var differentTriedAnswers = [];
        for (var i = 0; i < this.AnswersSoFar.length; i++) {
            if ($.inArray(this.AnswersSoFar[i], choices) !== -1 &&
                $.inArray(this.AnswersSoFar[i], differentTriedAnswers) === -1) {
                differentTriedAnswers.push(this.AnswersSoFar[i]);
            }
        }
        if (differentTriedAnswers.length + 1 === choices.length) {
            return true;
        }
        return false;
    }

    private countAnswerAsCorrect() {
        var self = this;
        var interactionNumber = $('#hddInteractionNumber').val();
        if (!this.AtLeastOneWrongAnswer) {
            interactionNumber++; //If no answer was given yet a new anser entry with interactionNumber has to be created
            this.IncrementInteractionNumber();
        }

        var url = this.AtLeastOneWrongAnswer
            ? AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect
            : AnswerQuestion.ajaxUrl_CountUnansweredAsCorrect;
        var successMessage = this.AtLeastOneWrongAnswer
            ? "Deine letzte Antwort wurde als richtig gewertet."
            : "Die Frage wurde als richtig beantwortet gewertet.";

        $.ajax({
            type: 'POST',
            url: url,
            data: {
                questionViewGuid: $('#hddQuestionViewGuid').val(),
                interactionNumber: interactionNumber,
                millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($.now())
            },
            cache: false,
            success: function(result) {
                self.AnswerCountedAsCorrect = true;
                $(Utils.UIMessageHtml(successMessage, "success")).insertAfter('#Buttons');
                self._inputFeedback.AnimateCorrectAnswer();
                $('#aCountAsCorrect').hide();
                $("#answerHistory").empty();
                $.post("/AnswerQuestion/PartialAnswerHistory",
                    { questionId: AnswerQuestion.GetQuestionId() },
                    function(data) {
                        $("#answerHistory").html(data);
                    });
            }
        });
    }

    private IsAnswerPossible() {

        if ($("#buttons-first-try").is(":visible"))
            return true;

        if ($("#buttons-edit-answer").is(":visible"))
            return true;

        if ($("#buttons-answer-again").is(":visible"))
            return true;

        return false;
    }

    public OnAnswerChange() {
        this.Renewable_answer_button_if_renewed_answer();
    }

    public Renewable_answer_button_if_renewed_answer() {
        if ($("#buttons-edit-answer").is(":visible")) {
            $("#buttons-edit-answer").hide();
            $("#buttons-answer-again").show();
            this._inputFeedback.AnimateNeutral();
        }
    }

    public GiveSelectedSolutionClass(event) {
        var changedButton = $(event.delegateTarget);
        changedButton.parent().parent().toggleClass("selected");
    }

    static AjaxGetSolution(onSuccessAction) {

        var self = this;

        $.ajax({
            type: 'POST',
            url: AnswerQuestion.ajaxUrl_GetSolution,
            data: {
                questionViewGuid: $('#hddQuestionViewGuid').val(),
                interactionNumber: $('#hddInteractionNumber').val(),
                millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($.now())
            },
            cache: false,
            success: result => {
                onSuccessAction(result);
            }
        });
    }

    private IncrementInteractionNumber() {
        $('#hddInteractionNumber')
            .val(function(i, oldval) {
                return (parseInt(oldval, 10) + 1).toString();
            });
    }

    static TimeSinceLoad(time: any) {
        return parseInt(time) - parseInt($('#hddTimeRecords').attr('data-time-on-load'));
    }

    static LogTimeForQuestionView() {
        $.ajax({
            type: 'POST',
            url: "/AnswerQuestion/LogTimeForQuestionView/",
            data: {
                questionViewGuid: $('#hddQuestionViewGuid').val(),
                millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($.now())
            }
        });
    }
}