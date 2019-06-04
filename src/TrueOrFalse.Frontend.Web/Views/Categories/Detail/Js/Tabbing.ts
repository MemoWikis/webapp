class Tabbing {

    private _page: CategoryPage ;

    constructor(page: CategoryPage) {

        this._page = page;

        $('#TabsBar .Tab').each((index, item) => {

            var tab = $(item);
            var tabname = tab.attr('id');

            tab.click((e) =>  {

                e.preventDefault();

                if (tab.hasClass('active'))
                    return;

                if (tab.hasClass('LoggedInOnly') && NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg(tabname);
                    return;
                }

                if (!this.ContentIsPresent(tabname)) {
                    Utils.ShowSpinner();
                    this.RenderTabContent(tabname);
                }
                if (tabname === "LearningTab" && $('#hddLearningSessionStarted').val() === "False" && $('#hddQuestionCount').val() !== "0") {
                    var answerBody = new AnswerBody();

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
        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + this._page.CategoryId;

        $.get(url, (html) => {

            Utils.HideSpinner();

            $('#' + tabName + 'Content')
                .empty()
                .append(html);
            if (tabName == "AnalyticsTab") {
                this.loadKnowledgeData();
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

    private loadKnowledgeData() {
        $('#KnowledgeGraph .knowledgeGraphData').html('<div style="text-align: center"><i class="fa fa-spinner fa-spin"></i></div>');
        $.ajax({
            url: '/Category/GetKnowledgeGraphDisplay?categoryId=' + $('#hhdCategoryId').val(),
            type: 'GET',
            success: data => {
                $('#KnowledgeGraph .knowledgeGraphData')
                    .html(data);
            }
        });

    }
}
