class Tabbing {

    private _reloadPins: boolean = false;
    private _categoryId: number;

    constructor(categoryId) {
        this._categoryId = categoryId;

        if (window.location.pathname.indexOf("/Lernen") >= 0) {
            $("#LearningTabContent").css("visibility", "visible");
            Utils.ShowSpinner();
            $("#LearnOptionsHeader").removeClass("disable");

            $('#hddLearningSessionStarted').val("True");

            $(() => {
                $("#TabContent .show-tooltip").tooltip();
            });
        }

        $("#LearnOptionsHeader").on("click", () => {
            if (window.location.pathname.indexOf("/Lernen") >= 0) 
                eventBus.$emit('openLearnOptions');
        }); 

        $('#TabsBar .Tab').each((index, item) => {

            var tab = $(item);
            var tabName = tab.attr('id');

            if (tabName == "LearningTabWithOptions")
                return;

            tab.click((e) => {
                if (window.location.pathname.indexOf("/Lernen") >= 0)
                    $("#LearnOptionsHeader").removeClass("disable");
                else
                    $("#LearnOptionsHeader").addClass("disable");

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

                if (tabName === "AnalyticsTab")
                   

                Utils.ShowSpinner();
                this.RenderTabContent(tabName);

                if (tabName === "LearningTab" && $('#hddLearningSessionStarted').val() === "False" && $('#hddQuestionCount').val() !== "0") {
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
        if (tabName == "LearningTabWithOptions")
            return;

        if (tabName == "LearningTabWithOptions")
            tabName = "LearningTab";

        var url = "/Category/Tab/?tabName=" + tabName + "&categoryId=" + this._categoryId;
        $.get(url, (html) => {
            Utils.HideSpinner();

            $('#' + tabName + 'Content').empty().append(html);

            if (tabName == "LearningTab" && $('#hddLearningSessionStarted').val() == "False" && $('#hddQuestionCount').val() != 0) {
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

            if (tabName == "TopicTab" ) {
                new Pin(PinType.Category, KnowledgeBar.ReloadCategory);
                this._reloadPins = false;
            }
        });
    }

    private ContentIsPresent(tabName: string): boolean {
        if (tabName == "LearningTabWithOptions")
            tabName = "LearningTab";

        return !($.trim($('#' + tabName + 'Content').html())=='');
    }

    private ShowTab(tabName: string): void {
        $('.Tab').removeClass('active');
        if (tabName == "LearningTabWithOptions" || tabName == "LearningTab") {
            $('#LearningTabWithOptions').addClass('active');
            tabName = "LearningTab";
        } else
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
