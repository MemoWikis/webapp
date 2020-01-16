enum PinType {
    Question,
    Set,
    SetDetail,
    Category,
    CategoryDetail
}

class Pin {

    _changeInProgress: boolean;
    _pinRowType: PinType;
    _isInLearningTab: boolean;

    OnPinChanged: () => void;

    constructor(pinRowType: PinType, onPinChanged: () => void = null) {

        var self = this; 
        this._pinRowType = pinRowType;
        this.OnPinChanged = onPinChanged;
        this._isInLearningTab = $('#LearningTab').length > 0;

        var allPins;
        if (self.IsQuestionRow())
            allPins = $(".Pin[data-question-id]").find(".iAdded, .iAddedNot"); 
        else if (self.IsSetDetail() || self.IsSetRow())
            allPins = $(".Pin[data-set-id]").find(".iAdded, .iAddedNot"); 
        else if (self.IsCategoryRow() || self.IsSetDetail())
            allPins = $(".Pin[data-category-id]").find(".iAdded, .iAddedNot");

        allPins.off('click.rowPin').on('click.rowPin', function (e) {

            e.preventDefault();
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("Pin_" + PinType[self._pinRowType]);
                return;
            }

            var elemPin = $($(this).parents(".Pin"));

            var id = -1;
            if (self.IsQuestionRow())
                id = parseInt(elemPin.attr("data-question-id"));
            else if (self.IsSetRow() || self.IsSetDetail())
                id = parseInt(elemPin.attr("data-set-id"));
            else if (self.IsCategoryRow() || self.IsCategoryDetail())
                id = parseInt(elemPin.attr("data-category-id"));

            if (self.IsCategoryRow()) {
                elemPin = $($.unique(elemPin.add($('[data-category-id=' + id + ']')).get()));//CategoryDetail page: toggle both desktop and mobile pin
            }

            if (this._changeInProgress)
                return;

            self._changeInProgress = true;

            if ($(this).hasClass("iAddedNot")) /* pin */ {

                self.Pin(id, onPinChanged, elemPin);
                elemPin.find(".iAddedNot, .iAddSpinner").toggleClass("hide2");

                window.setTimeout(() => {
                    elemPin.find(".iAdded, .iAddSpinner").toggleClass("hide2");
                    self._changeInProgress = false;

                    if (self.IsQuestionRow())
                        Utils.MenuPinsPluseOne();
                    else 
                        Utils.SetMenuPins();

                    if (self.IsSetDetail())
                        Utils.PinQuestionsInSetDetail();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) + 1).toString());

                    self.SetSidebarValue(self.GetSidebarValue(elemPin) + 1, elemPin);

                    if ($('.RoundHeartButton'))
                        self.SetFooterTotalPins(1);

                }, 400);

            } else /* unpin */ {

                self.UnPin(id, onPinChanged, elemPin);
                elemPin.find(".iAdded, .iAddSpinner").toggleClass("hide2");

                window.setTimeout(() => {
                    elemPin.find(".iAddedNot, .iAddSpinner").toggleClass("hide2");
                    self._changeInProgress = false;

                    if (self.IsQuestionRow()) 
                        Utils.MenuPinsMinusOne();

                    Utils.SetElementValue(".tabWishKnowledgeCount", (parseInt($(".tabWishKnowledgeCount").html()) - 1).toString());
                        
                    self.SetSidebarValue(self.GetSidebarValue(elemPin) - 1, elemPin);

                    if ($('.RoundHeartButton'))
                        self.SetFooterTotalPins(-1);

                }, 400);
            }
        });

        $("#UnpinSetModal").off("click").on("click", "#JS-RemoveQuestions", () => {
            $.when(
                SetsApi.UnpinQuestionsInSet($('#JS-RemoveQuestions').attr('data-set-id'), onPinChanged))
             .then(() => {
                Utils.UnpinQuestionsInSetDetail();
                Utils.SetMenuPins();
            });
        });

        $(".RoundHeartButton.iAdded").hover(hoverIn, hoverOut);

        function hoverIn() {
            $("i.fa-heart").addClass("onHover");
            $("i.fa-times").addClass("onHover");
            setTimeout(function () {
                $("i.fa-heart").addClass("hide2");
                $("i.fa-times").removeClass("hide2");
            }, 300);
        }
        function hoverOut() {
            $("i.fa-heart").removeClass("onHover");
            $("i.fa-times").removeClass("onHover");
            setTimeout(function () {
                $("i.fa-heart").removeClass("hide2");
                $("i.fa-times").addClass("hide2");
            }, 300);
        }

        $("#UnpinCategoryModal").off("click").on("click", "#JS-RemoveQuestionsCat", () => {
            $.when(
                CategoryApi.UnpinQuestionsInCategory($('#JS-RemoveQuestionsCat').attr('data-category-id'), onPinChanged))
            .then(() => {
                Utils.SetMenuPins();
                if (this._isInLearningTab)
                    this.UpdateWishknowledgeCount(-1, true);
            });
        });

        $("#DismissUnpinModal").on("click", () => {
                $('#UnpinCategoryModal').modal('hide');
        });
    }

    SetSidebarValue(newValue: number, parent: JQuery) {
        if (this.IsSetDetail()) {
            Utils.SetElementValue2(parent.find("#totalPins"), newValue.toString() + "x");
            parent.find("#totalPins").attr("data-original-title", "Ist bei " + newValue.toString() + " Personen im Wunschwissen");
        } else {
            Utils.SetElementValue2(parent.parents(".rowBase").find(".totalPins"), newValue.toString() + "x");
            parent.parents(".rowBase").find(".totalPinsTooltip").attr("data-original-title", "Ist bei " + newValue.toString() + " Personen im Wunschwissen");
        }
    }

    GetSidebarValue(parent: JQuery): number {
        if (this.IsSetDetail())
            return parseInt(/[0-9]*/.exec(parent.find("#totalPins").html())[0]);
        else
            return parseInt(/[0-9]*/.exec(parent.parents(".rowBase").find(".totalPins").html())[0]);
    }

    SetFooterTotalPins(n: number) {
        var totalPins = $('#CategoryFooterTotalPins').text();
        var newTotalPins = parseInt(totalPins, 10) + n;

        $('#CategoryFooterTotalPins').text(newTotalPins);
    }

    Pin(id: number, onPinChanged: () => void = null, elemPin = null) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Pin(id, onPinChanged);
            this.UpdateWishknowledgeCount(1 , false, id, elemPin);
        } else if (this.IsSetRow() || this.IsSetDetail()) {
            SetsApi.Pin(id, onPinChanged);
        } else if (this.IsCategoryRow() || this.IsCategoryDetail()) {
            CategoryApi.Pin(id, onPinChanged);
            if (this._isInLearningTab)
                this.UpdateWishknowledgeCount(1, true);
        }
    }

    UnPin(id: number, onPinChanged: () => void = null, elemPin = null) {
        if (this.IsQuestionRow()) {
            QuestionsApi.Unpin(id, onPinChanged);
            this.UpdateWishknowledgeCount(-1, false, id, elemPin);
        } else if (this.IsSetRow() || this.IsSetDetail()) {

            SetsApi.Unpin(id);

            $("#JS-RemoveQuestions").attr("data-set-id", id);
            $("#UnpinSetModal").modal('show');

        } else if (this.IsCategoryRow() || this.IsCategoryDetail()) {

            CategoryApi.Unpin(id);

            $("#JS-RemoveQuestionsCat").attr("data-category-id", id);
            $("#UnpinCategoryModal").modal('show');
        }
    }

    IsQuestionRow(): boolean {
        return this._pinRowType == PinType.Question;
    }

    IsSetRow(): boolean {
        return this._pinRowType == PinType.Set;
    }

    IsSetDetail(): boolean {
        return this._pinRowType == PinType.SetDetail;
    }

    IsCategoryRow(): boolean {
        return this._pinRowType == PinType.Category;
    }

    IsCategoryDetail(): boolean {
        return this._pinRowType == PinType.CategoryDetail;
    }

    async UpdateWishknowledgeCount(modifier, withCategory = false, questionId = null, elemPin = null) {

        var answerQuestionBodyMenu = $('.AnswerQuestionBodyMenu span[data-question-id]');

        if (withCategory) {
            await updateAnswerBody();
            var hasQuestionList = $('#QuestionListApp');
            if (hasQuestionList) {
                $('.questionListPinContainer span[data-question-id]').each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
                $('.questionDetailsStatistic span[data-question-id]').each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
            }

        }

        if (questionId == null)
            return;

        var questionDetails = $(".questionDetailsStatistic span.Pin[data-question-id='" + questionId + "']");
        var questionInList = $(".questionListPinContainer span.Pin[data-question-id='" + questionId + "']");

        updateQuestionDetails();

        function updateAnswerBody() {
            if (answerQuestionBodyMenu != undefined) {
                questionId = parseInt((answerQuestionBodyMenu).data('question-id'));
            }
        }

        function updateQuestionDetails() {
            if (withCategory) {
                updateHeartIcon(answerQuestionBodyMenu);
                questionDetails.each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
                questionInList.each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
            } else if (elemPin.parents('.AnswerQuestionBodyMenu').length > 0) {
                questionDetails.each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
                questionInList.each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
            } else if (elemPin.parents('.questionDetailsStatistic').length > 0) {
                var answerQuestionBodyWithSameQuestionId = $(".AnswerQuestionBodyMenu span[data-question-id='" + questionId + "']");
                if (answerQuestionBodyWithSameQuestionId)
                    updateHeartIcon(answerQuestionBodyWithSameQuestionId);
                questionInList.each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
            } else if (elemPin.parent('.questionListPinContainer').length > 0) {
                var answerQuestionBodyWithSameQuestionId = $(".AnswerQuestionBodyMenu span[data-question-id='" + questionId + "']");
                if (answerQuestionBodyWithSameQuestionId)
                    updateHeartIcon(answerQuestionBodyWithSameQuestionId);

                questionDetails.each(function () {
                    var el = $(this);
                    updateHeartIcon(el);
                });
            }

            var oldCount = parseInt($("span[data-question-details-id='" + questionId + "']").text());
            $("span[data-question-details-id='" + questionId + "']").text(oldCount + modifier);
        }

        function updateHeartIcon(el) {

            if (modifier == 1) {
                if (el.find('.iAdded').hasClass('hide2')) {

                    el.find('.iAddedNot').addClass('hide2');
                    el.find('.iAddSpinner').removeClass('hide2');

                    window.setTimeout(() => {
                        el.find('.iAddSpinner').addClass('hide2');
                        el.find('.iAdded').removeClass('hide2');
                    }, 400);
                }
            }
            else if (modifier == -1) {

                if (el.find('.iAddedNot').hasClass('hide2')) {

                    el.find('.iAdded').addClass('hide2');
                    el.find('.iAddSpinner').removeClass('hide2');

                    window.setTimeout(() => {
                        el.find('.iAddSpinner').addClass('hide2');
                        el.find('.iAddedNot').removeClass('hide2');
                    }, 400);
                }

            }
        }
    }
}