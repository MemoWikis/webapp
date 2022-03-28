
class AnswerBodyLoader {

    private _isInLearningTab: boolean;
    private _sessionConfigDataJson: any;
    private _getCustomSession: boolean = false;
    private _isSkipStep: boolean = false;


    constructor(answerBody: AnswerBody) {
        this._isInLearningTab = $('#LearningTab').length > 0;
        $(() => {
            if ($("#hddIsLearningSession").val() === "True") {

                if ($("#hddIsLearningSession").attr("data-learning-session-id") == "-1") {
                    $("#hddIsLearningSession").attr("data-learning-session-id", "-2");
                    this.loadNewLearningSession();
                }

                var self = this;
                $("#btnNext, #aSkipStep").click(function(e) {
                    e.preventDefault();
                    var skipstepIdx = this.id === "btnNext" ? -1 : 1;
                    self._isSkipStep = skipstepIdx !== -1;

                    var url = "/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                        "?skipStepIdx=" +
                        skipstepIdx;
                    AnswerQuestion.LogTimeForQuestionView();
                    self.loadNewQuestion(url);
                });

            } else {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByNextQuestion/?pager=" + pager;
                    AnswerQuestion.LogTimeForQuestionView();
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var pager = $("#PreviousQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByPreviousQuestion/?pager=" + pager;
                    AnswerQuestion.LogTimeForQuestionView();
                    this.loadNewQuestion(primaryDataUrl);
                });
            }

            if (Modernizr.history) {
                $(window).off("popstate");
                window.onpopstate = (event) => {
                    if ((event.state != null) && (event.state.stateType === "BrowserNavigation"))
                        location.reload();
                };
            }
        });
    }

    public loadNewTestSession() {
        this.loadNewSession();
    }

    public loadNewLearningSession(continueWithNewSession = false) {
        this.loadNewSession(null, false, continueWithNewSession);
    }

    public loadNewSession(questionFilter = null, loadedFromVue = false, continueWithNewSession = false, scrollToTop = true) {

        var base = {
            categoryId: $('#hhdCategoryId').val(),
            isInLearningTab: this._isInLearningTab,
        }
        var json = {};

        Object.keys(base)
            .forEach(key => json[key] = base[key]);

        if (questionFilter != null)
            Object.keys(questionFilter)
                .forEach(key => json[key] = questionFilter[key]);

        this._sessionConfigDataJson = json;

        var url = "/AnswerQuestion/RenderNewAnswerBodySessionForCategory";
        this._getCustomSession = true;
        this.loadNewQuestion(url, loadedFromVue, continueWithNewSession, true, scrollToTop);
    }

    public loadNewQuestion(url: string,
        loadedFromVue: boolean = false,
        continueWithNewSession: boolean = false,
        isNewSession: boolean = false,
        scrollToTop: boolean = true) {
        this._isInLearningTab = $('#LearningTab').length > 0;

        if (this._getCustomSession)
            $("#TestSessionHeader").remove();

        if (this._isInLearningTab && this._getCustomSession && loadedFromVue) {
            $("#AnswerBody").fadeOut();
            $("#QuestionDetails").fadeOut();
            $(".FooterQuestionDetails").fadeOut();
            $("#spanPercentageDone").html(0 + "%");
            $("#progressPercentageDone").width(0 + "%");
            Utils.ShowSpinner();
        }
        $.ajax({
            url: url,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(this._sessionConfigDataJson),
            type: 'POST',
            headers: { "cache-control": "no-cache" },
            success: result => {
                if (result !== "") {
                    result = JSON.parse(result);

                    if (result.LearningSessionResult) {
                        this.showLearningSessionResult(result);
                        $(".ProgressBarSegment .ProgressBarLegend").hide();
                        eventBus.$emit('set-session-progress', { isResult: true });
                        return;
                    }

                    if (result.counter.Max > 0) {
                        eventBus.$emit('destroy-answer-question-details');

                        if (!this._isInLearningTab)
                            this.updateUrl(result.url);

                        $(".FooterQuestionDetails").remove();
                        $("#AnswerBody").replaceWith(result.answerBodyAsHtml);

                        if (this._isInLearningTab && !this._getCustomSession)
                            $("#QuestionDetails").empty();

                        if ($("#hddIsLearningSession").val() !== "True")
                            this.updateNavigationBar(result.navBarData);

                        this.updateMenu(result.menuHtml);
                        document.title = $("#QuestionTitle").html();
                        $("div#comments").replaceWith(result.commentsAsHtml);

                        new AnswerBody();
                        initTooltips();
                        Images.Init();
                        initClickLog("div#LicenseQuestion");
                        initClickLog("div#AnswerBody");
                        initClickLog("div#AnswerQuestionPager");
                        initClickLog("div#answerQuestionDetails");
                        initClickLog("div#comments");
                        preventDropdownsFromBeingHorizontallyOffscreen("div#AnswerBody");

                        if (this._getCustomSession)
                            this._getCustomSession = false;

                        if ($("div[data-div-type='questionDetails']").length > 1)
                            $("div[data-div-type='questionDetails']").last().remove();

                        if ($("div[data-div-type='testSessionHeader']").length > 1)
                            $("div[data-div-type='testSessionHeader']").slice(1).remove();

                        if (continueWithNewSession) {
                            $("#QuestionListApp").fadeIn();
                        }

                        if (loadedFromVue) {
                            $("#AnswerBody").fadeIn();
                            $("#QuestionDetails").fadeIn();
                            $(".FooterQuestionDetails").fadeIn();
                            //$("#QuestionListApp").show(); 
                        }
                        if (scrollToTop)
                            this.scrollToTop();

                        if (isNewSession)
                            eventBus.$emit('init-new-session');

                        eventBus.$emit('set-session-progress', result.sessionData);

                        //$("#QuestionListApp").hide();
                        eventBus.$emit('change-active-question');
                        eventBus.$emit('update-question-list');
                    } else {
                        eventBus.$emit('set-session-progress', null);
                    }
                    eventBus.$emit('session-config-question-counter', result.counter);

                }
                    
                Utils.HideSpinner();
            },
            error: () => {
                Utils.HideSpinner();
            }
        });
    }

    private scrollToTop() {
        $('html, body').animate({ scrollTop: 0 }, 'fast');

        if (!Utils.IsInWidget()) {
            //set focus to first possible answer element
            if (document.getElementsByName("answer").length > 0)
                $("[name=answer]")[0].focus();

            $("#txtAnswer:visible").focus();

            $("#row-1:visible").focus();
        }
    }

    private showLearningSessionResult(result) {
        var container = this._isInLearningTab || $("#AnswerBody").length > 0
            ? $('#AnswerBody')
            : $("#MasterMainWrapper");
        container.html(result.LearningSessionResult);
        new LearningSessionResult();
    }

    private updateUrl(url: string) {
        if (Modernizr.history)
            history.pushState({ stateType: "BrowserNavigation" }, $(".QuestionText").html(), url);
    }

    private updateMenu(newMenuHtml: string) {
        if (newMenuHtml)
            $("#mainMenuThemeNavigation").replaceWith($(newMenuHtml));
    }

    private updateNavigationBar(navBarData: any) {
        $("#AnswerQuestionPager .Current").replaceWith($(navBarData.currentHtml).find(".Current"));

        if (navBarData.nextUrl) {
            if ($("#NextQuestionLink").length === 0)
                $("#AnswerQuestionPager .Next").append($(navBarData.currentHtml).find("#NextQuestionLink"));
            else
                $("#NextQuestionLink").attr("href", navBarData.nextUrl);
        } else {
            $("#NextQuestionLink").remove();
        }

        if (navBarData.previousUrl) {
            if ($("#PreviousQuestionLink").length === 0)
                $("#AnswerQuestionPager .Previous").append($(navBarData.currentHtml).find("#PreviousQuestionLink"));
            else
                $("#PreviousQuestionLink").attr("href", navBarData.previousUrl);
        } else {
            $("#PreviousQuestionLink").remove();
        }

        $("#NextQuestionLink, #PreviousQuestionLink").unbind();
    }
}