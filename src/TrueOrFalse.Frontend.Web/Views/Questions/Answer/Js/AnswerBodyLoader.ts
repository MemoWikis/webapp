
class AnswerBodyLoader {

    private _answerBody: AnswerBody;
    private _isInLearningTab: boolean;
    private _sessionConfigDataJson: SessionConfigDataJson;
    private _getCustomSession: boolean = false;

    constructor(answerBody: AnswerBody) {

        this._answerBody = answerBody;
        this._isInLearningTab = $('#LearningTab').length > 0;

        if (Utils.IsInWidget())
            return;

        $(() => {

            if (window.location.pathname.split("/")[4] === "im-Fragesatz") {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#NextQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyBySet/?questionId=" +
                        questionId +
                        "&setId=" +
                        setId;
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#PreviousQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyBySet/?questionId=" +
                        questionId +
                        "&setId=" +
                        setId;
                    this.loadNewQuestion(primaryDataUrl);
                });

            } else if ($("#hddIsLearningSession").val() === "True") {

                if ($("#hddIsLearningSession").attr("data-learning-session-id") == "-1")
                    this.loadNewLearningSession();

                $("#btnNext, #aSkipStep").click((e) => {
                    
                    e.preventDefault();
                    var learningSessionId = $("#hddIsLearningSession").attr("data-learning-session-id");
                    var skipStepIdx = $("#hddIsLearningSession").attr("data-skip-step-index");
                    var isInTestMode = $("#isInTestMode").val() == "True";
                    var url = "/AnswerQuestion/RenderAnswerBodyByLearningSession/?learningSessionId=" +
                        learningSessionId +
                        "&skipStepIdx=" +
                        skipStepIdx +
                        "&isInTestMode=" +
                        isInTestMode;
                    this.loadNewQuestion(url);
                });

            } else if (this._answerBody.IsTestSession()) {

                $("#btnNext").click((e) => {
                    e.preventDefault();
                    var testSessionId = $("#hddIsTestSession").attr("data-test-session-id");
                    var url = "/AnswerQuestion/RenderAnswerBodyByTestSession/?testSessionId=" + testSessionId;
                    this.loadNewQuestion(url);
                });

            } else {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByNextQuestion/?pager=" + pager;
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var pager = $("#PreviousQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByPreviousQuestion/?pager=" + pager;
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

    public loadNewLearningSession() {
        this.loadNewSession();
    }

    public loadNewSession(questionFilter = null, loadedFromVue = false) {

        this._sessionConfigDataJson = {
            categoryId: $('#hhdCategoryId').val(),
            isInLearningTab: this._isInLearningTab,
            questionFilter: questionFilter,
        }

        var url = "/AnswerQuestion/RenderNewAnswerBodySessionForCategory";
        this._getCustomSession = true;
        this.loadNewQuestion(url, loadedFromVue);
    }

    public loadNewQuestion(url: string, loadedFromVue: boolean = false) {
        this._isInLearningTab = $('#LearningTab').length > 0;
        if (this._getCustomSession)
            $("#TestSessionHeader").remove();
        if (this._isInLearningTab && this._getCustomSession && loadedFromVue) {
            $("#AnswerBody").fadeOut();
            $("#QuestionDetails").fadeOut();
            $(".FooterQuestionDetails").fadeOut();
            $(".SessionSessionHeading").fadeOut();
            $(".SessionBar").fadeOut();
            $("#spanPercentageDone").html(0 + "%");
            $("#progressPercentageDone").width(0 + "%");
            Utils.ShowSpinner();
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        }
        $.ajax({
            url: url,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(this._sessionConfigDataJson),
            type: 'POST',
            headers: { "cache-control": "no-cache" },
            success: result => {
                result = JSON.parse(result);
                if (!this._isInLearningTab) {
                    this.updateUrl(result.url);
                }
                //this.sendGoogleAnalyticsPageView(result.offlineDevelopment);
                if (result.LearningSessionResult) {
                    this.showLearningSessionResult(result);
                    $(".ProgressBarSegment .ProgressBarLegend").hide();
                    return;
                }
                $(".FooterQuestionDetails").remove();
                $("#modalShareQuestion").remove();
                $("#AnswerBody").replaceWith(result.answerBodyAsHtml);
                if (this._isInLearningTab && !this._getCustomSession) {
                    $("#QuestionDetails").empty();
                }

                if ($("#hddIsLearningSession").val() === "True" || this._answerBody.IsTestSession()) {
                    this.updateSessionHeader(result.sessionData);

                    if (result.sessionData.learningSessionId > -1)
                        $("#hddIsLearningSession").attr("data-learning-session-id", result.sessionData.learningSessionId);
                    if (result.sessionData.testSessionId > -1)
                        $("#hddIsTestSession").attr("data-test-session-id", result.sessionData.testSessionId);
                }
                else 
                    this.updateNavigationBar(result.navBarData);

                this.updateMenu(result.menuHtml);
                document.title = $(".QuestionText").html();
                $("#QuestionDetails").replaceWith(result.questionDetailsAsHtml);
                $("div#comments").replaceWith(result.commentsAsHtml);

                new AnswerBody();
                FillSparklineTotals();
                InitTooltips();
                Images.Init();
                InitClickLog("div#LicenseQuestion");
                InitClickLog("div#AnswerBody");
                InitClickLog("div#AnswerQuestionPager");
                InitClickLog("div#answerQuestionDetails");
                InitClickLog("div#comments");
                PreventDropdonwnsFromBeingHorizontallyOffscreen("div#AnswerBody");
                Utils.HideSpinner();
                if (this._getCustomSession)
                    this._getCustomSession = false;
                if ($("div[data-div-type='questionDetails']").length > 1)
                    $("div[data-div-type='questionDetails']").last().remove();
                if ($("div[data-div-type='testSessionHeader']").length > 1)
                    $("div[data-div-type='testSessionHeader']").slice(1).remove();

                if (IsLoggedIn.Yes) {
                    var node = $(".SessionType > span.show-tooltip");
                    var testToolTip = " <div style= 'text-align: left;'> In diesem Modus" +
                        "<ul>" +
                        "<li>werden die Fragen zufällig ausgewählt </li>" +
                        "<li>hast du jeweils nur einen Antwortversuch </li>" +
                        "</ul>" +
                        "</div>";
                    var learningToolTip = "<div style= 'text-align: left;'> In diesem Modus" +
                        "<ul>" +
                        "<li>wiederholst du personalisiert die Fragen, die du am dringendsten lernen solltest </li>" +
                        "<li>kannst du dir die Lösung anzeigen lassen </li>" +
                        "<li>werden dir Fragen, die du nicht richtig beantworten konntest, nochmal vorgelegt </li>" +
                        "</ul>" +
                        "</div>";
                    if (result.isInTestMode) {
                        node[0].firstChild.nodeValue = "Testen";
                        node[0].setAttribute("data-original-title", testToolTip);
                    }
                    else {
                        node[0].firstChild.nodeValue = "Lernen";
                        node[0].setAttribute("data-original-title", learningToolTip);
                    }
                }

                if (loadedFromVue) {
                    $(".SessionSessionHeading").fadeIn();
                    $(".SessionBar").fadeIn();
                    $("#AnswerBody").fadeIn();
                    $("#QuestionDetails").fadeIn();
                    $(".FooterQuestionDetails").fadeIn();
                }
            }
        });
    }

    private updateNavigationBar(navBarData: any) {
        $("#AnswerQuestionPager .Current").replaceWith($(navBarData.currentHtml).find(".Current"));

        if (navBarData.nextUrl) {
            if($("#NextQuestionLink").length === 0)
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

    private showLearningSessionResult(result) {
        var container = this._isInLearningTab || $("#AnswerBody").length > 0 ? $('#AnswerBody') : $("#MasterMainWrapper");
        container.html(result.LearningSessionResult);
        new LearningSessionResult();
    }

    private updateSessionHeader(sessionStepData) {
        if ($("#hddIsTestSession").val() === "True") {
            $("#hddIsTestSession").attr("data-current-step-idx", sessionStepData.currentStepIdx);
            $("#hddIsTestSession").attr("data-is-last-step", sessionStepData.isLastStep);
            $(".SessionBar .QuestionCount").html(sessionStepData.currentSessionHeader);
        }
        else if ($("#hddIsLearningSession").val() === "True") {
            $("#hddIsLearningSession").attr("data-current-step-guid", sessionStepData.currentStepGuid);
            $("#hddIsLearningSession").attr("data-current-step-idx", sessionStepData.currentStepIdx);
            $("#hddIsLearningSession").attr("data-skip-step-index", sessionStepData.skipStepIdx);
            $("#hddIsLearningSession").attr("data-is-last-step", sessionStepData.isLastStep);
            $(".SessionBar .QuestionCount").html(sessionStepData.currentSessionHeader);
        }   
    }

    private updateUrl(url: string) {
        if(Modernizr.history)
            history.pushState({stateType: "BrowserNavigation"}, $(".QuestionText").html(), url);
    }

    private updateMenu(newMenuHtml: string) {
        if(newMenuHtml)
            $("#mainMenuThemeNavigation").replaceWith($(newMenuHtml));
    }
}