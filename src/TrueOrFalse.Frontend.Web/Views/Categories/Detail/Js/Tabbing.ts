class Tabbing {

    private _page: CategoryPage;

    constructor(page: CategoryPage) {

        if (window.location.pathname.indexOf("/Lernen") >= 0) {
            var answerBody = new AnswerBody();
            $("#LearningTabContent").css("visibility", "visible");
            Utils.ShowSpinner();

            if (answerBody.IsTestSession()) {
                answerBody.Loader.loadNewTestSession();
            }

            $('#hddLearningSessionStarted').val("True");

            $(() => {
                $("#TabContent .show-tooltip").tooltip();
            });
        }

        $(window).on('popstate',
            (e) => {
                var state = e.originalEvent.state;
                console.log(state);
            });

        this._page = page;

        $('#TabsBar .Tab').each((index, item) => {

            var tab = $(item);
            var tabname = tab.attr('id');

            tab.click((e) =>  {

                e.preventDefault();

                //if (tab.hasClass('active'))
                //    return;

                //if (tab.hasClass('LoggedInOnly') && NotLoggedIn.Yes()) {
                //    NotLoggedIn.ShowErrorMsg(tabname);
                //    return;
                //}

                if (!this.ContentIsPresent(tabname)) {
                    Utils.ShowSpinner();
                    this.RenderTabContent(tabname);
                }
                if (tabname === "LearningTab" && $('#hddLearningSessionStarted').val() === "False" && $('#hddQuestionCount').val() !== "0") {
                    var answerBody = new AnswerBody();
                    if (!$("#LearningTabContent").css("visibility", "visible"))
                        $("#LearningTabContent").css("visibility", "visible");
                   
                    Utils.ShowSpinner();

                    if (answerBody.IsTestSession()) {
                        answerBody.Loader.loadNewTestSession();
                    }

                    $('#hddLearningSessionStarted').val("True");

                    $(() => {
                        $("#TabContent .show-tooltip").tooltip();
                    });
                }
                this.ShowTab(tabname);

            });
        });
    }

    private RenderTabContent(tabName: string): void {
        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + this._page.categoryId;

        $.get(url, (html) => {

            Utils.HideSpinner();

            if (tabName !== "TopicTab")
                $('#' + tabName + 'Content').empty().append(html);
            else 
                $('#ContentModuleApp').empty().append(html);
        });
    }

    private InitializeLearningTab(): void{
        var answerBody = new AnswerBody();

        if (answerBody.IsTestSession()) {
        answerBody.Loader.loadNewTestSession();
    }

    $('#hddLearningSessionStarted').val("True");
    }

    private ContentIsPresent(tabName: string): boolean {
        return false;  //!($.trim($('#' + tabName + 'Content').html())=='');
    }

    private ShowTab(tabName: string): void {

        $('.Tab').removeClass('active');
        $('#' + tabName).addClass('active');

        $('.TabContent').fadeOut(200);

        $('#' + tabName + "Content").delay(200).fadeIn(200);
    }
}
