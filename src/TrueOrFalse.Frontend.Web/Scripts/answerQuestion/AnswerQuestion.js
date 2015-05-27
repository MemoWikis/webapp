var answerResult;

var choices = [];

var AnswerQuestion = (function () {
    function AnswerQuestion(solutionEntry) {
        var _this = this;
        this.AnswersSoFar = [];
        this.AmountOfTries = 0;
        this.AtLeastOneWrongAnswer = false;
        this._isGameMode = solutionEntry.IsGameMode;

        this._getAnswerText = solutionEntry.GetAnswerText;
        this._getAnswerData = solutionEntry.GetAnswerData;
        this._onNewAnswer = solutionEntry.OnNewAnswer;

        AnswerQuestion.ajaxUrl_SendAnswer = $("#ajaxUrl_SendAnswer").val();
        AnswerQuestion.ajaxUrl_GetAnswer = $("#ajaxUrl_GetAnswer").val();
        AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect = $("#ajaxUrl_CountLastAnswerAsCorrect").val();

        this._inputFeedback = new AnswerQuestionUserFeedback(this);

        var self = this;

        $("#txtAnswer").keypress(function (e) {
            if (e.keyCode == 13) {
                if (self.IsAnswerPossible()) {
                    self.ValidateAnswer();
                    return false;
                }
            }
            return true;
        });

        $("#btnCheck").click(function (e) {
            e.preventDefault();
            self.ValidateAnswer();
        });

        $("#btnCheckAgain").click(function (e) {
            e.preventDefault();
            self.ValidateAnswer();
        });

        $("#btnCountAsCorrect").click(function (e) {
            e.preventDefault();
            self.countLastAnswerAsCorrect();
        });

        $("#CountWrongAnswers").click(function (e) {
            e.preventDefault();
            var divWrongAnswers = $("#divWrongAnswers");
            if (!divWrongAnswers.is(":visible"))
                divWrongAnswers.show();
            else
                divWrongAnswers.hide();
        });

        $(".selectorShowAnswer").click(function () {
            _this._inputFeedback.ShowCorrectAnswer();
            return false;
        });

        $("#buttons-edit-answer").click(function (e) {
            e.preventDefault();
            _this._onNewAnswer();

            _this._inputFeedback.AnimateNeutral();
        });
    }
    AnswerQuestion.GetQuestionId = function () {
        return +$("#questionId").val();
    };

    AnswerQuestion.prototype.ValidateAnswer = function () {
        var answerText = this._getAnswerText();
        var self = this;

        if (answerText.trim().length === 0) {
            $('#spnWrongAnswer').hide();
            self._inputFeedback.ShowError("Du könntest es ja wenigstens probieren ... (Wird nicht als Antwortversuch gewertet.)", true);
            return false;
        } else {
            $('#spnWrongAnswer').show();
            self.AmountOfTries++;
            self.AnswersSoFar.push(answerText);

            $("#answerHistory").html("<i class='fa fa-spinner fa-spin' style=''></i>");

            $.ajax({
                type: 'POST',
                url: AnswerQuestion.ajaxUrl_SendAnswer,
                data: self._getAnswerData(),
                cache: false,
                success: function (result) {
                    answerResult = result;
                    $("#buttons-first-try").hide();
                    $("#buttons-answer-again").hide();

                    if (result.correct) {
                        self._inputFeedback.ShowSuccess();
                        self._inputFeedback.ShowCorrectAnswer(false);
                    } else {
                        if (self._isGameMode) {
                            self._inputFeedback.ShowErrorGame();
                            self._inputFeedback.ShowCorrectAnswer(false);
                        } else {
                            self._inputFeedback.UpdateAnswersSoFar();

                            self.AtLeastOneWrongAnswer = true;
                            self._inputFeedback.ShowError();

                            if (result.choices != null) {
                                choices = result.choices;
                                if (self.allWrongAnswersTried(answerText)) {
                                    self._inputFeedback.ShowCorrectAnswer();
                                }
                            }
                        }
                    }
                    ;

                    $("#answerHistory").empty();
                    $.post("/AnswerQuestion/PartialAnswerHistory", { questionId: AnswerQuestion.GetQuestionId() }, function (data) {
                        $("#answerHistory").html(data);
                    });
                }
            });
            return false;
        }
    };

    AnswerQuestion.prototype.allWrongAnswersTried = function (answerText) {
        var differentTriedAnswers = [];
        for (var i = 0; i < this.AnswersSoFar.length; i++) {
            if ($.inArray(this.AnswersSoFar[i], choices) !== -1 && $.inArray(this.AnswersSoFar[i], differentTriedAnswers) === -1) {
                differentTriedAnswers.push(this.AnswersSoFar[i]);
            }
        }
        if (differentTriedAnswers.length + 1 === choices.length) {
            return true;
        }
        return false;
    };

    AnswerQuestion.prototype.countLastAnswerAsCorrect = function () {
        var self = this;
        $.ajax({
            type: 'POST',
            url: AnswerQuestion.ajaxUrl_CountLastAnswerAsCorrect,
            cache: false,
            success: function (result) {
                $(Utils.UIMessageHtml("Deine letzte Antwort wurde als richtig gewertet.", "success")).insertBefore('#Buttons');
                $('#btnCountAsCorrect').attr('disabled', 'true');
                $("#answerHistory").empty();
                $.post("/AnswerQuestion/PartialAnswerHistory", { questionId: AnswerQuestion.GetQuestionId() }, function (data) {
                    $("#answerHistory").html(data);
                });
            }
        });
    };

    AnswerQuestion.prototype.IsAnswerPossible = function () {
        if ($("#buttons-first-try").is(":visible"))
            return true;

        if ($("#buttons-edit-answer").is(":visible"))
            return true;

        if ($("#buttons-answer-again").is(":visible"))
            return true;

        return false;
    };

    AnswerQuestion.prototype.OnAnswerChange = function () {
        this.Renewable_answer_button_if_renewed_answer();
    };

    AnswerQuestion.prototype.Renewable_answer_button_if_renewed_answer = function () {
        if ($("#buttons-edit-answer").is(":visible")) {
            $("#buttons-edit-answer").hide();
            $("#buttons-answer-again").show();
            this._inputFeedback.AnimateNeutral();
        }
    };

    AnswerQuestion.AjaxGetAnswer = function (onSuccessAction) {
        $.ajax({
            type: 'POST',
            url: this.ajaxUrl_GetAnswer,
            cache: false,
            success: function (result) {
                onSuccessAction(result);
            }
        });
    };
    return AnswerQuestion;
})();
//# sourceMappingURL=AnswerQuestion.js.map
