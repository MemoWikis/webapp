class AnswerQuestionUserFeedback_FlashCard {

    private _answerQuestion: AnswerQuestion_FlashCard;

    constructor(answerQuestion: AnswerQuestion_FlashCard) {
        this._answerQuestion = answerQuestion;
    }

    ShowSolution() {

        this.ShowNextQuestionLink();
        $("#txtAnswer").attr('disabled', 'true').addClass('disabled');
        if (this._answerQuestion.SolutionType !== SolutionType.MatchList &&
            this._answerQuestion.SolutionType !== SolutionType.MultipleChoice) {
            if (this._answerQuestion.AnswersSoFar.length === 1) {
                $("#divWrongAnswers .WrongAnswersHeading").html('Deine Antwort:');
                if ($("#txtAnswer").val() !== this._answerQuestion.AnswersSoFar[0]) {
                    $("#divWrongAnswers").show();
                }
            }
            if (this._answerQuestion.AnswersSoFar.length > 1) {
                $("#divWrongAnswers .WrongAnswersHeading").html('Deine Antworten:');
                $("#divWrongAnswers").show();
            }
        }
        this.RenderSolutionDetails();
    }

    RenderSolutionDetails() {
        $('#AnswerInputSection').find('.radio').addClass('disabled').find('input').attr('disabled', 'true');
        if (this._answerQuestion.SolutionType === SolutionType.MatchList) {
            $('#AnswerInputSection .ui-droppable')
                .each((index, matchlistDropElement) => {
                    $(matchlistDropElement).droppable('disable');
                });
            $('#AnswerInputSection .ui-draggable')
                .each((index, matchlistDragElement) => {
                    $(matchlistDragElement).draggable('disable');
                });
            $(".matchlist-mobileselect").addClass('disabled').attr('disabled', 'true');
        }

        $('#Buttons').css('visibility', 'hidden');
        window.setTimeout(function () { $("#SolutionDetailsSpinner").show(); }, 500);

        AnswerQuestion.AjaxGetSolution(result => {

            if (this._answerQuestion.IsTestSession && this._answerQuestion.AnswersSoFar.length === 0) {
                //if solution is shown after answering the question in a TestSession, then this does not need to be registered
                //otherwise, solutionview needs to be registered in the current TestSessionStep
                $.ajax({
                    type: 'POST',
                    url: AnswerQuestion.ajaxUrl_TestSessionRegisterAnsweredQuestion,
                    data: {
                        testSessionId: AnswerQuestion.TestSessionId,
                        questionId: AnswerQuestion.GetQuestionId(),
                        questionViewGuid: $('#hddQuestionViewGuid').val(),
                        answeredQuestion: false
                    },
                    cache: false
                });

                this._answerQuestion.UpdateProgressBar(this._answerQuestion.GetCurrentStep());

                AnswerQuestionUserFeedback.IfLastQuestion_Change_Btn_Text_ToResult();
            }

            if (this._answerQuestion.IsLearningSession && this._answerQuestion.AnswersSoFar.length === 0) {
                //if is learningSession and user asked to show solution before answering, then queue this question to be answered again
                var self = this;
                this._answerQuestion.ShowedSolutionOnly = true;
                $.ajax({
                    type: 'POST',
                    url: AnswerQuestion.ajaxUrl_LearningSessionAmendAfterShowSolution,
                    data: {
                        learningSessionId: this._answerQuestion.LearningSessionId,
                        stepGuid: this._answerQuestion.LearningSessionStepGuid
                    },
                    cache: false,
                    success(result) {
                        self._answerQuestion.UpdateProgressBar(result.numberSteps);
                    }
                });
            }


            if (this._answerQuestion.SolutionType === SolutionType.MultipleChoice && !result.correctAnswer) {
                $("#Solution").show().find('.Label').html("Keine der Antworten ist richtig!");
            } else {
                var shownCorrectAnswer = result.correctAnswerAsHTML;
                $("#Solution").show().find('.Content').html("</br>" + shownCorrectAnswer);
            }
            if (this._answerQuestion.SolutionType === SolutionType.MultipleChoice || this._answerQuestion.SolutionType === SolutionType.MultipleChoice_SingleSolution)
                this.HighlightMultipleChoiceSolution(result.correctAnswer);
            if (this._answerQuestion.SolutionType === SolutionType.MatchList)
                this.HighlightMatchlistSoluion(result.correctAnswer);
            
            if (result.correctAnswerDesc) {
                $("#Description").show().find('.Content').html(result.correctAnswerDesc);
            }
            if (result.correctAnswerReferences.length > 0) {
                $("#References").show();
                var indexSuccessfulReferences = 0;
                $(window).on('oneMoreReference', () => {
                    indexSuccessfulReferences++;
                    if (indexSuccessfulReferences === result.correctAnswerReferences.length) {
                        this.ShowAnswerDetails();
                    }
                });
                for (var i = 0; i < result.correctAnswerReferences.length; i++) {
                    var reference = result.correctAnswerReferences[i];
                    var referenceHtml = $('<div class="ReferenceDetails"></div>');
                    referenceHtml.appendTo('#References .Content');

                    var fnRenderReference = function (div, ref) {
                        if (ref.referenceText) {
                            if (ref.referenceType == 'UrlReference') {
                                $('<div class="ReferenceText"><a href="' + ref.referenceText + '" target="_blank">' + ref.referenceText + '</a></div>').appendTo(div);
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
                                fnRenderReference(div, ref);

                                $('.show-tooltip').tooltip();
                                $(window).trigger('oneMoreReference');
                            }
                        });
                    }

                    if (reference.categoryId != -1) {
                        fnAjaxCall(referenceHtml, reference);
                    } else {
                        fnRenderReference(referenceHtml, reference);
                        $(window).trigger('oneMoreReference');
                    }
                }
            } else {
                this.ShowAnswerDetails();
            }
        });
    }

    static IfLastQuestion_Change_Btn_Text_ToResult() {
        if (AnswerQuestion.IsLastTestSessionStep) {
            $('#btnNext').html('Zum Ergebnis');
        }
    }

    private ShowNextQuestionLink() {

        if (!this._answerQuestion.IsLastQuestion()) {
            $("#buttons-next-question").show();
        } else {
            $("#buttons-next-question").hide();
        }

        if (!this._answerQuestion.AnsweredCorrectly &&
            !this._answerQuestion.IsGameMode) {
            $("#aCountAsCorrect").show();
        }

        $("#answerFeedbackTry").hide();

        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();
    }
} 