var choices = [];

class AnswerQuestion_FlashCard {

    private _onCorrectAnswer: () => void = () => {};
    private _onWrongAnswer: () => void = () => {};

    private _inputFeedback: AnswerQuestionUserFeedback_FlashCard;
    private _isLastLearningStep = false;

    static ajaxUrl_SendAnswer: string;
    static ajaxUrl_GetSolution: string;
    static ajaxUrl_CountLastAnswerAsCorrect: string;
    static ajaxUrl_CountUnansweredAsCorrect: string;
    static ajaxUrl_TestSessionRegisterAnsweredQuestion: string;
    static ajaxUrl_LearningSessionAmendAfterShowSolution: string;
    static TestSessionProgressAfterAnswering: number;

    static IsLastTestSessionStep = false;
    static TestSessionId: number;

    public LearningSessionId: number;
    public LearningSessionStepGuid: string;

    public SolutionType: SolutionType;
    public IsGameMode: boolean;
    public IsLearningSession = false;
    public IsTestSession = false;

    public AnsweredCorrectly = false;
    //public AnswersSoFar = [];
    public AmountOfTries = 0;
    //public AtLeastOneWrongAnswer = false;
    //public AnswerCountedAsCorrect = false;
    public ShowedSolutionOnly = false;

    constructor(answerEntry: IAnswerEntry) {

        this.SolutionType = answerEntry.SolutionType;
        this.IsGameMode = answerEntry.IsGameMode;
        if ($('#hddIsLearningSession').length === 1)
            this.IsLearningSession = $('#hddIsLearningSession').val().toLowerCase() === "true";

        if (this.IsLearningSession) {
            this.LearningSessionId = +$('#hddIsLearningSession').attr('data-learning-session-id');
            this.LearningSessionStepGuid = $('#hddIsLearningSession').attr('data-current-step-guid');
        }

        if (this.IsLearningSession && $('#hddIsLearningSession').attr('data-is-last-step'))
            this._isLastLearningStep = $('#hddIsLearningSession').attr('data-is-last-step').toLowerCase() === "true";

        if ($('#hddIsTestSession').length === 1)
            this.IsTestSession = $('#hddIsTestSession').val().toLowerCase() === "true";

        if (this.IsTestSession && $('#hddIsTestSession').attr('data-is-last-step'))
            AnswerQuestion.IsLastTestSessionStep = $('#hddIsTestSession').attr('data-is-last-step').toLowerCase() === "true";

        if (this.IsTestSession && $('#hddIsTestSession').attr('data-test-session-id'))
            AnswerQuestion.TestSessionId = parseInt($('#hddIsTestSession').attr('data-test-session-id'));


        AnswerQuestion.ajaxUrl_SendAnswer = $("#ajaxUrl_SendAnswer").val();
        AnswerQuestion.ajaxUrl_GetSolution = $("#ajaxUrl_GetSolution").val();
        AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect = $("#ajaxUrl_CountLastAnswerAsCorrect").val();
        AnswerQuestion.ajaxUrl_CountUnansweredAsCorrect = $("#ajaxUrl_CountUnansweredAsCorrect").val();
        AnswerQuestion.ajaxUrl_TestSessionRegisterAnsweredQuestion = $("#ajaxUrl_TestSessionRegisterAnsweredQuestion").val();
        AnswerQuestion.ajaxUrl_LearningSessionAmendAfterShowSolution = $("#ajaxUrl_LearningSessionAmendAfterShowSolution").val();
        AnswerQuestion.TestSessionProgressAfterAnswering = $("#TestSessionProgessAfterAnswering").val();

        this._inputFeedback = new AnswerQuestionUserFeedback_FlashCard(this);

        var self = this;

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

        $("#btnRightAnswer")
            .click(
            e => {
                e.preventDefault();
                //do sth here
            });

        $("#btnWrongAnswer")
            .click(
            e => {
                e.preventDefault();
                //do sth here
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

    private RightAnswer() {
        //do here on right answer
    }

    private WrongAnswer() {
        //do here on wrong answer
    }

    private ValidateAnswer() {
        var self = this;

        $('#flashcard-btnNext').show();
            self.AmountOfTries++;
            $.ajax({
                type: 'POST',
                url: AnswerQuestion_FlashCard.ajaxUrl_SendAnswer,
                data: $.extend(self._getAnswerData(),
                {
                    questionViewGuid: $('#hddQuestionViewGuid').val(),
                    interactionNumber: $('#hddInteractionNumber').val(),
                    millisecondsSinceQuestionView: AnswerQuestion
                        .TimeSinceLoad($('#hddTimeRecords').attr('data-time-of-answer'))
                }),
                cache: false,
                success(result) {
                    var answerResult = result;
                    self.IncrementInteractionNumber();

                    self.UpdateProgressBar(-1, answerResult);

                    if (result.correct)
                        self.HandleCorrectAnswer();
                    else 
                        self.HandleWrongAnswer(result, answerText);
                }
            });
            return false;
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

    //private allWrongAnswersTried(answerText: string) {
    //    var differentTriedAnswers = [];
    //    for (var i = 0; i < this.AnswersSoFar.length; i++) {
    //        if ($.inArray(this.AnswersSoFar[i], choices) !== -1 &&
    //            $.inArray(this.AnswersSoFar[i], differentTriedAnswers) === -1) {
    //            differentTriedAnswers.push(this.AnswersSoFar[i]);
    //        }
    //    }
    //    if (differentTriedAnswers.length + 1 === choices.length) {
    //        return true;
    //    }
    //    return false;
    //}

    //private countAnswerAsCorrect() {
    //    var self = this;
    //    var interactionNumber = $('#hddInteractionNumber').val();
    //    if (!this.AtLeastOneWrongAnswer) {
    //        interactionNumber++; //If no answer was given yet a new anser entry with interactionNumber has to be created
    //        this.IncrementInteractionNumber();
    //    }

    //    var url = this.AtLeastOneWrongAnswer
    //        ? AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect
    //        : AnswerQuestion.ajaxUrl_CountUnansweredAsCorrect;
    //    var successMessage = this.AtLeastOneWrongAnswer
    //        ? "Deine letzte Antwort wurde als richtig gewertet."
    //        : "Die Frage wurde als richtig beantwortet gewertet.";

    //    $.ajax({
    //        type: 'POST',
    //        url: url,
    //        data: {
    //            questionViewGuid: $('#hddQuestionViewGuid').val(),
    //            interactionNumber: interactionNumber,
    //            millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($.now()),
    //            testSessionId: AnswerQuestion.TestSessionId,
    //            learningSessionId: self.LearningSessionId,
    //            learningSessionStepGuid: self.LearningSessionStepGuid
    //        },
    //        cache: false,
    //        success: function(result) {
    //            self.AnswerCountedAsCorrect = true;
    //            $(Utils.UIMessageHtml(successMessage, "success")).insertAfter('#Buttons');
    //            self._inputFeedback.AnimateCorrectAnswer();
    //            $('#aCountAsCorrect').hide();
    //            $("#answerHistory").empty();
    //            $.post("/AnswerQuestion/PartialAnswerHistory",
    //                { questionId: AnswerQuestion.GetQuestionId() },
    //                function(data) {
    //                    $("#answerHistory").html(data);
    //                });

    //            self.UpdateProgressBar(self.GetCurrentStep() - 1);

    //            if (self._isLastLearningStep)
    //                $('#btnNext').html('Zum Ergebnis');
    //        }
    //    });
    //}

    //private IsAnswerPossible() {

    //    if ($("#buttons-first-try").is(":visible"))
    //        return true;

    //    if ($("#buttons-answer-again").is(":visible"))
    //        return true;

    //    return false;
    //}

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

    UpdateProgressBar(numberSteps : number = -1, answerResult : any = null) {

        var raiseTo: number;
        var percentage: number = parseInt($("#spanPercentageDone").html());
        var stepNumberChanged: boolean;
        var numberStepsDone: number = parseInt($('#CurrentStepNumber').html());
        var numberStepsUpdated = numberSteps !== -1 ? numberSteps : answerResult.numberSteps;

        if (this.IsTestSession) {
            raiseTo = +AnswerQuestion.TestSessionProgressAfterAnswering;
        } else if (this.IsLearningSession) {
            raiseTo = Math.round(numberStepsDone / numberStepsUpdated * 100);
            stepNumberChanged = this.GetCurrentStep() != numberStepsUpdated;
            if (stepNumberChanged) {
                $("#StepCount").fadeOut(100);
            }
        } else {return;}

        $("#spanPercentageDone").fadeOut(100);
        var intervalId = window.setInterval(ChangePercentage, 10);

        function ChangePercentage() {
            
            if (percentage === raiseTo) {

                window.clearInterval(intervalId);

                $("#spanPercentageDone").html(raiseTo + "%");
                $("#spanPercentageDone").fadeIn();
                if (stepNumberChanged) {
                    $("#StepCount").html(numberStepsUpdated);
                    $("#StepCount").fadeIn();
                }

            } else {

                if(percentage < raiseTo)
                    percentage++;
                else 
                    percentage--;
                    
                $("#progressPercentageDone").width(percentage + "%");
            }
        }
    }

    GetCurrentStep() : number {
        return parseInt($("#StepCount").html());
    }
}