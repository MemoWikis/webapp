class AnswerQuestionUserFeedback {

    private _errMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter, nicht aufgeben.",
        "Übung macht den Meister. Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
    ];

    private _successMsgs = ["Yeah! Weiter so.", "Du bist auf einem guten Weg.", "Sauber!", "Well Done!"];

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
            $("#answerFeedback").html(text).show();
        } else {
            $("#answerFeedback").html(text).hide();
        }

        this.AnimateWrongAnswer();
    }

    AnimateWrongAnswer() {
        $("#buttons-edit-answer").show();
        $("#txtAnswer").animate({ backgroundColor: "#efc7ce" }, 1000);
    }

    AnimateNeutral() {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);
    }

    ShowSuccess() {
        var self = this;

        $("#divAnsweredCorrect").show();
        $("#buttons-next-answer").show();
        $("#buttons-edit-answer").hide();
        $("#txtAnswer").animate({ backgroundColor: "#D1EBA7" }, 1000);
        $("#divWrongAnswer").hide();

        $("#divAnsweredCorrect").show();
        $("#wellDoneMsg").html("" + self._successMsgs[Utils.Random(0, self._successMsgs.length - 1)]).show();

        this.RenderAnswerDetails();
    }

    ShowCorrectAnswer(showNextAnswerButton : boolean = true) {

        this.ShowNextAnswer();

        if (!this._answerQuestion.AtLeastOneWrongAnswer) {
            $("#txtAnswer").hide();
        }

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
        this.RenderAnswerDetails();
    }

    RenderAnswerDetails() {
        $('#AnswerInputSection').find('.radio').addClass('disabled').find('input').attr('disabled', 'true');
        $('#Buttons').css('visibility', 'hidden');
        window.setTimeout(function () { $("#SolutionDetailsSpinner").show(); }, 1000);

        AnswerQuestion.AjaxGetAnswer(result => {

            $("#Solution").show().find('.Content').html(result.correctAnswer);
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

    ShowAnswerDetails() {
        window.setTimeout(function () {
            $("#SolutionDetailsSpinner").remove();
            $("#SolutionDetails").show();
            $('#Buttons').css('visibility', 'visible');
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

    private ShowNextAnswer() {

        $("#buttons-next-answer").show();
        if (this._answerQuestion.AtLeastOneWrongAnswer) {
            $("#btnCountAsCorrect").removeAttr('disabled').show();
        }

        $("#answerFeedback").hide();

        $("#buttons-first-try").hide();
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").hide();
    }
} 