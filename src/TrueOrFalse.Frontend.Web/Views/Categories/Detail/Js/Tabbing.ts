class Tabbing {

    private _reloadPins: boolean = false;
    private _categoryId :number;
    constructor(categoryId) {
        this._categoryId = categoryId;

        if (window.location.pathname.indexOf("/Lernen") >= 0) {
            var answerBody = new AnswerBody();
            $("#LearningTabContent").css("visibility", "visible");
            Utils.ShowSpinner();

            $('#hddLearningSessionStarted').val("True");

            $(() => {
                $("#TabContent .show-tooltip").tooltip();
            });
        }

        $('#TabsBar .Tab').each((index, item) => {

            var tab = $(item);
            var tabName = tab.attr('id');

            tab.click((e) =>  {

                e.preventDefault();
                if (tab.hasClass('LoggedInOnly') && NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg(tabName);
                    return;
                }

                if (!this.ContentIsPresent(tabName)) {
                    Utils.ShowSpinner();
                    this.RenderTabContent(tabName);
                }

                this.ShowTab(tabName);
            });
        });

        $('#LearningFooterBtn, #AnalyticsFooterBtn').on('click', () => {
                var currentTarget = $(event.currentTarget);
                var tabName = currentTarget.attr('data-tab-id');

                var gaEventLabel = 'LearningTab-footer';
                if (tabName === "AnalyticsTab")
                    gaEventLabel = 'AnalyticsTab-footer';

                Utils.ShowSpinner();
                this.RenderTabContent(tabName);

                if (tabName === "LearningTab" && $('#hddLearningSessionStarted').val() === "False" && $('#hddQuestionCount').val() !== "0") {
                    var answerBody = new AnswerBody();

                    Utils.ShowSpinner();

                    $('#hddLearningSessionStarted').val("True");

                    $(() => {
                        $("#TabContent .show-tooltip").tooltip();
                    });
                }
                this.ShowTab(tabName);
            }
        );
    }

    public RenderTabContent(tabName: string): void {
        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + this._categoryId;

        $.get(url, (html) => {
            Utils.HideSpinner();
            $('#' + tabName + 'Content').empty().append(html);

            if (tabName == "LearningTab" && $('#hddLearningSessionStarted').val() == "False" && $('#hddQuestionCount').val() != 0) {
                var answerBody = new AnswerBody();
                if (!$("#LearningTabContent").css("visibility", "visible"))
                    $("#LearningTabContent").css("visibility", "visible");

                Utils.ShowSpinner();

                $('#hddLearningSessionStarted').val("True");

                $(() => {
                    $("#TabContent .show-tooltip").tooltip();
                });
            }
            else if (tabName == "AnalyticsTab") {
                this.loadKnowledgeData();
            }

            if (tabName == "TopicTab" && this._reloadPins ) {
                new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
                this._reloadPins = false;
            }
        });


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
