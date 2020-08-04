﻿var choices = [];

class AnswerQuestion {
    private ClickToContinue: () => void;
    private _getAnswerText: () => string;
    private _getAnswerData: () => {};

    private _onCorrectAnswer: () => void = () => {};
    private _onWrongAnswer: () => void = () => { };

    private _inputFeedback: AnswerQuestionUserFeedback;
    public _isLastLearningStep = false;

    static ajaxUrl_SendAnswer: string;
    static ajaxUrl_GetSolution: string;
    static ajaxUrl_CountLastAnswerAsCorrect: string;
    static ajaxUrl_CountUnansweredAsCorrect: string;
    static ajaxUrl_TestSessionRegisterAnsweredQuestion: string;
    static ajaxUrl_LearningSessionAmendAfterShowSolution: string;
    static TestSessionProgressAfterAnswering: number;
    private IsLandingPage: boolean = false; 

    public LearningSessionStepGuid: string;

    public SolutionType: SolutionType;
    public IsGameMode: boolean;
    public IsLearningSession = false;

    public AnsweredCorrectly = false;
    public AnswersSoFar = [];
    public AmountOfTries = 0;
    public AtLeastOneWrongAnswer = false;
    public AnswerCountedAsCorrect = false;
    public ShowedSolutionOnly = false;

    constructor(answerEntry: IAnswerEntry) {
        this.IsLandingPage = $("#hddIsLandingPage").val() == "2";
        this.SolutionType = answerEntry.SolutionType;
        this.IsGameMode = answerEntry.IsGameMode;

        if ($('#hddIsLearningSession').length === 1)
            this.IsLearningSession = $('#hddIsLearningSession').val().toLowerCase() === "true";

        if (this.IsLearningSession) {
            this.LearningSessionStepGuid = $('#hddIsLearningSession').attr('data-current-step-guid');
        }

        if (this.IsLearningSession && $('#hddIsLearningSession').attr('data-is-last-step').toLowerCase() === "true")
            this._isLastLearningStep = true ;

        this._getAnswerText = () => { return answerEntry.GetAnswerText(); }
        this._getAnswerData = () => { return answerEntry.GetAnswerData(); }

        AnswerQuestion.ajaxUrl_GetSolution = $("#ajaxUrl_GetSolution").val();
        AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect = $("#ajaxUrl_CountLastAnswerAsCorrect").val();
        AnswerQuestion.ajaxUrl_CountUnansweredAsCorrect = $("#ajaxUrl_CountUnansweredAsCorrect").val();
        AnswerQuestion.ajaxUrl_TestSessionRegisterAnsweredQuestion = $("#ajaxUrl_TestSessionRegisterAnsweredQuestion").val();
        AnswerQuestion.ajaxUrl_LearningSessionAmendAfterShowSolution = $("#ajaxUrl_LearningSessionAmendAfterShowSolution").val();
        AnswerQuestion.TestSessionProgressAfterAnswering = $("#TestSessionProgessAfterAnswering").val();

        this._inputFeedback = new AnswerQuestionUserFeedback(this);

        var self = this;
        this.ClickToContinue = function () {
            $('#continue').fadeIn();
            $(document).on('click',
                '.clickToContinue',
                (e) => {
                    e.preventDefault();
                    setVideo.HideYoutubeOverlay();
                    player.playVideo();
                });
        };

        $('body').keydown(function(e) {                                                       // simulate Click wenn sichbar und wenn Enter gedrückt wird
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
                self.ValidateAnswer();
            });

        $("#aCountAsCorrect")
            .click(
                e => {
                    e.preventDefault();
                    self.countAnswerAsCorrect();
                    ActivityPoints.addPointsFromCountAsCorrect();
                   
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
                ActivityPoints.addPointsFromShowSolutionAnswer();
                this.ClickToContinue(); 
                if ($('#ActivityPointsContainer'))
                    self.GetActivityPointsForSidebar();
                return false;
            });

        $("#btnNext, #aSkipStep")
            .click((e) => {
                if (this.IsLearningSession && this.AmountOfTries === 0 && !this.AnswerCountedAsCorrect && !this.ShowedSolutionOnly) {
                    $("#hddIsLearningSession").attr("data-skip-step-index", $('#hddIsLearningSession').attr('data-current-step-idx'));
                    questionListApp.activeQuestion = parseInt($('#hddIsLearningSession').attr('data-current-step-idx')) + 1;
                    console.log(questionListApp.currentStep);                } else {
                    $("#hddIsLearningSession").attr("data-skip-step-index", -1);
                    questionListApp.activeQuestion = parseInt($('#hddIsLearningSession').attr('data-current-step-idx')) + 1;
                }
            });

        $("#aSkipStep").click(e => {
            this.UpdateProgressBar(this.GetCurrentSteps());
        });

        this.FlashCardCheck();
        this.HideChipsOnLandingPageAndDisplayCards();
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
        return $("#isLastQuestion").val() === "True"; // ??? check if needed !!!
    }

    public ValidateAnswer() {
        var answerText = this._getAnswerText();
        var self = this;

        if (answerText.trim().length === 0 &&
            this.SolutionType !== SolutionType.MultipleChoice &&
            this.SolutionType !== SolutionType.MatchList &&
            this.SolutionType !== SolutionType.FlashCard) {
            $('#spnWrongAnswer').hide();
            self._inputFeedback
                .ShowError("Du könntest es ja wenigstens probieren ... (Wird nicht als Antwortversuch gewertet.)",
                    true);
            return false;
        } else {
            self.AmountOfTries++;
            self.AnswersSoFar.push(answerText);

            if (this.SolutionType !== SolutionType.FlashCard) {
                $('#spnWrongAnswer').show();
                $("#buttons-first-try").hide();
                $("#buttons-answer-again").hide();
                // $('#continue').hide();
                $("#answerHistory").html("<i class='fa fa-spinner fa-spin' style=''></i>");
            } else {
                $('#buttons-answer').hide();
                this.ClickToContinue();
            }
            $('div#answerFeedbackTry, a#CountWrongAnswers').hide();
            var isTestMode = $("input[name='r1']:checked").val() == "Test";
            $.ajax({
                type: 'POST',
                url: "/AnswerQuestion/SendAnswerLearningSession",
                data: $.extend(self._getAnswerData(),
                    {
                        id: $("#hddQuestionId").val(),
                        questionViewGuid: $('#hddQuestionViewGuid').val(),
                        interactionNumber: $('#hddInteractionNumber').val(),
                        millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($('#hddTimeRecords').attr('data-time-of-answer')),
                    inTestMode: isTestMode
                    }),
                cache: false,
                success(result) {
                    $('div#answerFeedbackTry, a#CountWrongAnswers').show();
                    var answerResult = result;
                    self.IncrementInteractionNumber();
                    self.UpdateProgressBar(-1, answerResult);

                    if (result.correct) {
                        self.HandleCorrectAnswer();
                        self.ClickToContinue();
                    } else {
                        $('div#answerFeedbackTry, a#CountWrongAnswers').show();
                        self.HandleWrongAnswer(result, answerText);
                    }

                    $("#answerHistory").empty();
                    $.post("/AnswerQuestion/PartialAnswerHistory",
                        { questionId: AnswerQuestion.GetQuestionId() },
                        function(data) {
                            $("#answerHistory").html(data);
                        });
                    self.updateQuestionDetails();
                    eventBus.$emit('reload-correctnessprobability-for-question', AnswerQuestion.GetQuestionId());
                    KnowledgeSummaryBar.updateKnowledgeSummaryBar();
                    if ($('#ActivityPointsContainer'))
                        self.GetActivityPointsForSidebar();
                }
            });
            return false;
        }
    }

    private HideChipsOnLandingPageAndDisplayCards() {
        if (this.IsLandingPage) {

            if (window.innerWidth < 768) {
                $(".row  .question-details").children().first().hide(); //byLoad from the Page
                $("#SlowerThen768").css("display", "block");
            } else {
                $("#GreaterThen767").css("display", "block");
            }


            var ishidden = false;
            var isFadeIn = false;
            $(window).on("resize",
                () => {
                    if (window.innerWidth < 768 &&
                        $(".row  .question-details").children().first().css("display") != "none" && !ishidden) {
                        $(".row  .question-details").children().first().hide();
                        $("#SlowerThen768").css("display", "block");
                        $("#GreaterThen767").css("display", "none");
                        ishidden = true;
                        isFadeIn = false;
                    } else if (window.innerWidth > 767 && $(".row  .question-details").children().first().css("display") == "none" && !isFadeIn) {
                        $(".row  .question-details").children().first().fadeIn(500);
                        $("#SlowerThen768").css("display", "none");
                        $("#GreaterThen767").css("display", "block");
                        ishidden = false;
                        isFadeIn = true;
                    }

                });
        } else {
            $("#GreaterThen767").css("display", "block");
        }
    }

    private HandleCorrectAnswer() {
        this.AnsweredCorrectly = true;

        ActivityPoints.addPointsFromRightAnswer();

        if (this.SolutionType !== SolutionType.FlashCard) {
            this._inputFeedback.ShowSuccess();
        }
        this._inputFeedback.ShowSolution();
        if (this._isLastLearningStep) {
            $('#btnNext').html('Zum Ergebnis');
        }

        this._onCorrectAnswer();
    }

    private HandleWrongAnswer(result: any, answerText: string) {
        ActivityPoints.addPointsFromWrongAnswer();

        if (this._isLastLearningStep && !result.newStepAdded) {
            $('#btnNext').html('Zum Ergebnis');
        }

        if (this.IsGameMode) {
            if (this.SolutionType !== SolutionType.FlashCard) {
                this._inputFeedback.ShowSolution();
            }
        } else {
            if (this.SolutionType === SolutionType.FlashCard) {
                this._inputFeedback.ShowSolution();
            }
            this._inputFeedback.UpdateAnswersSoFar();
            this.RegisterWrongAnswer();//need only this
            this._inputFeedback.ShowError();
            
            if (this.IsLearningSession) {

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

    private updateQuestionDetails() {
        var questionId = AnswerQuestion.GetQuestionId();
        eventBus.$emit('set-question-id', questionId);
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

        var url = this.AtLeastOneWrongAnswer ? AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect : AnswerQuestion.ajaxUrl_CountUnansweredAsCorrect;
        var successMessage = this.AtLeastOneWrongAnswer ? "Deine letzte Antwort wurde als richtig gewertet." : "Die Frage wurde als richtig beantwortet gewertet.";

        $.ajax({
            type: 'POST',
            url: url,
            data: {
                questionViewGuid: $('#hddQuestionViewGuid').val(),
                interactionNumber: interactionNumber,
                millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($.now()),
                learningSessionStepGuid: self.LearningSessionStepGuid
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

                self.UpdateProgressBar(self.GetCurrentSteps() - 1);

                if (self._isLastLearningStep) {
                    $('#btnNext').html('Zum Ergebnis');
                }
                self.updateQuestionDetails();
                eventBus.$emit('reload-correctnessprobability-for-question', AnswerQuestion.GetQuestionId());
                KnowledgeSummaryBar.updateKnowledgeSummaryBar();
                if ($('#ActivityPointsContainer'))
                    self.GetActivityPointsForSidebar();
            }
        });
    }

    static AjaxGetSolution(onSuccessAction) {

        $.ajax({
            type: 'POST',
            url: AnswerQuestion.ajaxUrl_GetSolution,
            data: {
                questionViewGuid: $('#hddQuestionViewGuid').val(),
                interactionNumber: $('#hddInteractionNumber').val(),
                millisecondsSinceQuestionView: AnswerQuestion.TimeSinceLoad($.now()),
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

    UpdateProgressBar(numberSteps: number = -1, answerResult: any = null, isReaddet = false) {

        var raiseTo: number;
        var percentage: number = parseInt($("#spanPercentageDone").html());
        var stepNumberChanged: boolean;
        var numberStepsDone: number = parseInt($('#CurrentStepNumber').html());
        var numberStepsUpdated = numberSteps !== -1 ? numberSteps : answerResult.numberSteps;
        if (this.IsLearningSession) {
            raiseTo = isReaddet ? Math.round(numberStepsDone / (numberStepsUpdated + 1) * 100) : Math.round(numberStepsDone / (numberStepsUpdated) * 100);
            stepNumberChanged = this.GetCurrentSteps() != numberStepsUpdated;
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

    GetCurrentSteps() : number {
        return parseInt($("#StepCount").html());
    }

    private GetActivityPointsForSidebar() {
        $.ajax({
            url: "/Shared/RenderActivityPopupContent",
            type: 'GET',
            async: false,
            success: function (htmlResult) {
                $('#ActivityPointsContainer').html(htmlResult);
            }
        });
    }

    private FlashCardCheck() {
        var answerSection = $('#AnswerAndSolution .row');
        var hasFlashCard = answerSection.has('#AnswerInputSection #flashCardContent');
        if (hasFlashCard)
            answerSection.addClass('hasFlashCard');
        else
            answerSection.removeClass('hasFlashCard');
    }
}