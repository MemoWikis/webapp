class AnswerQuestionUserFeedback {

    private _errMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter, nicht aufgeben.",
        "Übung macht den Meister. Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
    ];

    private _successMsgs = ["Yeah!", "Du bist auf einem guten Weg.", "Sauber!", "Well Done!", "Toll!", "Weiter so!", "Genau.", "Absolut.",
                            "Richtiger wird's nicht.", "Fehlerlos!", "Korrrrrekt!", "Einwandfrei", "Mehr davon!", "Klasse.", "Schubidu!",
                            "Wer sagt's denn!", "Exakt.", "So ist es.", "Da kannste nicht meckern.", "Sieht gut aus.", "Oha!", "Rrrrrrichtig!"];

    private _answerQuestion: AnswerQuestion;

    constructor(answerQuestion: AnswerQuestion) {
        this._answerQuestion = answerQuestion;
    }

    public ShowErrorGame() {
        $("#divWrongAnswerPlay").show();
        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();

        $("#divWrongEnteredAnswer").html(this._answerQuestion.AnswersSoFar[0]);

        this.AnimateWrongAnswer();
    }

    public ShowError(text = "", forceShow: boolean = false) {

        if (text === "") {
            text = this._errMsgs[Utils.Random(0, this._errMsgs.length - 1)];
        }

        this.UpdateAnswersSoFar();

        $("#divWrongAnswer").show();
        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();

        if (forceShow || Utils.Random(1, 10) % 4 === 0) {
            $("#answerFeedbackTry").html(text).show();
        } else {
            $("#answerFeedbackTry").html(text).hide();
        }

        this.AnimateWrongAnswer();
    }

    AnimateWrongAnswer() {
        $("#buttons-edit-answer").show();
        $(".answerFeedbackWrong").fadeIn(1200, function() {
             $(this).fadeOut(800);
        });
    }
    AnimateCorrectAnswer() {
        $(".answerFeedbackCorrect").fadeIn(1200, function () {
            $(this).fadeOut(800);
        });
    }

    AnimateNeutral() {
        //$("#txtAnswer").animate({ backgroundColor: "white" }, 200);
    }

    ShowSuccess() {
        var self = this;

        $("#divAnsweredCorrect").show();
        $("#buttons-next-question").show();
        $("#buttons-edit-answer").hide();
        this.AnimateCorrectAnswer();
        $("#divWrongAnswer").hide();

        $("#divAnsweredCorrect").show();
        $("#wellDoneMsg").html("" + self._successMsgs[Utils.Random(0, self._successMsgs.length - 1)]).show();
    }

    ShowSolution() {

        this.ShowNextQuestionLink();
        //check if Solution Type == multiple Choice and then execute HighlightMultipleChoiceSolution()
        $("#txtAnswer").attr('disabled', 'true').addClass('disabled');
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
        this.RenderSolutionDetails();
    }

    RenderSolutionDetails() {
        $('#AnswerInputSection').find('.radio').addClass('disabled').find('input').attr('disabled', 'true');
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

                if (AnswerQuestion.IsLastTestSessionStep) {
                    $('#btnNext').html('Zum Ergebnis');
                }
            }

            if (this._answerQuestion.IsLearningSession && this._answerQuestion.AnswersSoFar.length === 0) {
                //if is learningSession and user asked to show solution before answering, then queue this question to be answered again
                var self = this;
                this._answerQuestion.ShowedSolutionOnly = true;
                $.ajax({
                    type: 'POST',
                    url: AnswerQuestion.ajaxUrl_LearningSessionAmendAfterShowSolution,
                    data: {
                        learningSessionId: $('#hddIsLearningSession').attr('data-learning-session-id'),
                        stepGuid: $('#hddIsLearningSession').attr('data-current-step-guid')
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
                $("#Solution").show().find('.Content').html(result.correctAnswer);
            }
            if (this._answerQuestion.SolutionType === SolutionType.MultipleChoice || this._answerQuestion.SolutionType === SolutionType.MultipleChoice_SingleSolution)
                this.HighlightMultipleChoiceSolution(result.correctAnswer);
            
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

    private HighlightMultipleChoiceSolution(correctAnswers: string) {
        var allAnswerElements = $("input[name = 'answer']");

        for (var i = 0; i < allAnswerElements.length; i++) {

            var currentElement = $(allAnswerElements.get(i));

            if (correctAnswers.indexOf(currentElement.val()) !== -1) {
                currentElement.parent().addClass("right-answer");
            } else {
                if (currentElement.prop("checked")) {
                    currentElement.parent().addClass("wrong-answer");      
                }
            } 
        }
    }

    ShowAnswerDetails() {
        window.setTimeout(function () {
            $("#SolutionDetailsSpinner").remove();
            $("#SolutionDetails").show();
            $('#Buttons').css('visibility', 'visible');
            $('#hddTimeRecords').attr('data-time-of-solution-view', $.now());
        }, 50);
    }

    UpdateAnswersSoFar() {

        var errorTryText;
        var amountOfTriesText = ["0 Versuche", "ein Versuch", "zwei", "drei", "vier", "fünf", "sehr hartnäckig", "Respekt!"];

        switch (this._answerQuestion.AmountOfTries) {
            case 0:
            case 1:
                errorTryText = amountOfTriesText[this._answerQuestion.AmountOfTries]; break;
            case 2:
            case 3:
            case 4:
            case 5:
                errorTryText = amountOfTriesText[this._answerQuestion.AmountOfTries] + " Versuche"; break;
            case 6:
            case 7:
                errorTryText = amountOfTriesText[this._answerQuestion.AmountOfTries]; break;
            default:
                errorTryText = amountOfTriesText[7];
        }
        $("#CountWrongAnswers").html("(" + errorTryText + ")");

        $('#ulAnswerHistory').html("");

        $.each(this._answerQuestion.AnswersSoFar, function (index, val) {
            $('#ulAnswerHistory').append(
                $('<li>' + val + '</li>'));
        });
    }

    private ShowNextQuestionLink() {

        if (!this._answerQuestion.IsLastQuestion()) {
            $("#buttons-next-question").show();
        } else {
            $("#buttons-next-question").hide();
        }

        if (!this._answerQuestion.AnsweredCorrectly &&
            !this._answerQuestion.IsGameMode &&
            !this._answerQuestion.IsLearningSession &&
            !this._answerQuestion.IsTestSession) {
            $("#aCountAsCorrect").show();
        }

        $("#answerFeedbackTry").hide();

        $("#buttons-first-try").hide();
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").hide();
    }
} 