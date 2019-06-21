class Tabbing {

    private _categoryId :number;
    constructor(categoryId) {
        this._categoryId = categoryId;

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

      

        $('#TabsBar .Tab').each((index, item) => {

            var tab = $(item);
            var tabname = tab.attr('id');

            tab.click((e) =>  {

                e.preventDefault();
                if (tab.hasClass('LoggedInOnly') && NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg(tabname);
                    return;
                }

                if (!this.ContentIsPresent(tabname)) {
                    Utils.ShowSpinner();
                    this.RenderTabContent(tabname);
                }

                this.ShowTab(tabname);
            });
        });
    }

    public RenderTabContent(tabName: string): void {
        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + this._categoryId;
        console.log(this._categoryId);

        $.get(url, (html) => {
            Utils.HideSpinner();
            $('#' + tabName + 'Content').empty().append(html);

            if (tabName === "LearningTab" && $('#hddLearningSessionStarted').val() === "False" && $('#hddQuestionCount').val() !== "0") {
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
        return !($.trim($('#' + tabName + 'Content').html())=='');
    }

    private ShowTab(tabName: string): void {

        $('.Tab').removeClass('active');
        $('#' + tabName).addClass('active');

        $('.TabContent').fadeOut(200);

        $('#' + tabName + "Content").delay(200).fadeIn(200);
    }
}
