interface QuestionListItem
{
    CorrectnessProbability: Number;
    HasPersonalAnswer: Boolean;
    Id: Number;
    ImageData: String;
    IsInWishknowledge: Boolean;
    LearningSessionStepCount: Number;
    LinkToComment: String;
    LinkToDeleteQuestion: String;
    LinkToEditQuestion: String;
    LinkToQuestion: String;
    LinkToQuestionDetailSite: String;
    LinkToQuestionVersions: String;
    Title: String;
    Visibility: Number;
}

Vue.component('question-list-component', {
    components: {
        VueSlider: window['vue-slider-component']
    },
    props: [
        'categoryId',
        'isAdmin',
        'isQuestionListToShow',
        'activeQuestionId',
        'selectedPageFromActiveQuestion',
        'questionCount'],
    data() {
        return {
            pages: 0,
            selectedPage: 1,
            itemCountPerPage: 25,
            questions: [] as QuestionListItem[],
            hasQuestions: false,
            showFirstPage: true,
            pageArray: [] as Number[],
            questionText: "Fragen",
            showLeftPageSelector: false,
            showRightPageSelector: false,
            leftSelectorArray: [] as Number[],
            rightSelectorArray: [] as Number[],
            centerArray: [] as Number[],
            showLeftSelectionDropUp: false,
            showRightSelectionDropUp: false,
            pageIsLoading: false,
            lastQuestionInListIndex: null,
            answerBodyHasLoaded: false,
        };
    },
    created() {
        eventBus.$on('reload-knowledge-state', () => this.initQuestionList());
        eventBus.$on('reload-wishknowledge-state-per-question', (data) => this.changeQuestionWishknowledgeState(data.questionId, data.isInWishknowledge));
        eventBus.$on('reload-correctnessprobability-for-question', (id) => this.getUpdatedCorrectnessProbability(id));
        eventBus.$on('add-question-to-list', (q: QuestionListItem) => { this.addQuestionToList(q)});
    },
    mounted() {
        this.categoryId = $("#hhdCategoryId").val();
        eventBus.$on('question-deleted', (data) => {
            this.questions = this.questions.filter(q => {
                return q.Id != data.id;
            });

            if (this.questions.length <= 0)
                this.hasQuestions = false;

            var answerBody = new AnswerBody();
            var skipIndex = this.questions != null ? -5 : 0;
            var index = this.questions.length > 0 && data.index === -1 ? 0 : data.index;
            if (this.questions.length >= 1)
                answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                    "?skipStepIdx=" +
                    skipIndex +
                    "&index=" +
                    index);

            eventBus.$emit('change-active-question');
        });
        eventBus.$on('update-question-list', () => this.initQuestionList());
    },
    watch: {
        questionCount() {
            this.updatePageCount(this.selectedPage);
        },
        itemCountPerPage(val) {
            this.pages = Math.ceil(this.questionCount / val);
        },
        selectedPageFromActiveQuestion(val) {
            this.selectedPage = val;
        },
        questions() {
            if (this.questions.length > 0)
                this.hasQuestions = true;

            if (this.questions.length == 1)
                this.questionText = "Frage";

            if (this.questions.length <= 0) {
                this.hasQuestions = false;

                $('#SessionHeaderContainer').replaceWith(
                    "<div id='NoQuestionsSessionBar' class='NoQuestions' style='margin-top: 40px;'>" +
                        "Es sind leider noch keine Fragen zum Lernen in diesem Thema enthalten." +
                    "</div>"
                );

                $("#AnswerBody").html(
                    "<input type='hidden' id='hddSolutionTypeNum' value='1'>" +
                    "<div id='QuestionDetails' data-div-type='questionDetails'></div>"
                );
            }

        },
        selectedPage(val) {
            this.loadQuestions(val);
        },
        pages(val) {
            let newArray = [];

            let startNumber = 1;
            for (let i = 0; startNumber < val + 1; i++) {
                newArray.push(startNumber);
                startNumber = startNumber + 1;
            }
            this.pageArray = newArray;

            if (val < this.selectedPage)
                this.selectedPage = val;
        },
        leftSelectorArray() {
            if (this.leftSelectorArray.length >= 2) {
                this.showLeftPageSelector = true;
            }
            else
                this.showLeftPageSelector = false;
        },
        rightSelectorArray() {
            if (this.rightSelectorArray.length >= 2) {
                this.showRightPageSelector = true;
            }
            else
                this.showRightPageSelector = false;
        },
    },
    methods: {
        initQuestionList() {
            this.loadQuestions(this.selectedPage);
        },
        loadQuestions(selectedPage) {
            this.pageIsLoading = true;
            $.ajax({
                url: "/QuestionList/LoadQuestions/",
                data: {
                    itemCountPerPage: this.itemCountPerPage,
                    pageNumber: selectedPage,
                },
                type: "POST",
                success: questions => {
                    this.questions = questions;
                    this.updatePageCount(selectedPage);
                    eventBus.$emit('update-question-count');
                },
            });
        },
        updatePageCount(selectedPage) {
            this.selectedPage = selectedPage;
            this.showLeftSelectionDropUp = false;
            this.showRightSelectionDropUp = false;
            if (typeof this.questions[0] != "undefined")
                this.pages = Math.ceil(this.questionCount / this.itemCountPerPage);
            else
                this.pages = 1;
            this.$nextTick(function () {
                this.setPaginationRanges(selectedPage);
                new Pin(PinType.Question);
            });
            this.pageIsLoading = false;
        },
        setPaginationRanges(selectedPage) {
            if ((selectedPage - 2) <= 2) {
                this.hideLeftPageSelector = true;
            };
            if ((selectedPage + 2) >= this.pageArray.length) {
                this.hideRightPageSelector = true;
            };

            let leftArray = [];
            let centerArray = [];
            let rightArray = [];

            if (this.pageArray.length >= 8) {

                centerArray = _.range(selectedPage - 2, selectedPage + 3);
                centerArray = centerArray.filter(e => e >= 2 && e <= this.pageArray.length - 1);

                leftArray = _.range(2, centerArray[0]);
                rightArray = _.range(centerArray[centerArray.length - 1] + 1, this.pageArray.length);

                this.centerArray = centerArray;
                this.leftSelectorArray = leftArray;
                this.rightSelectorArray = rightArray;    

            } else {
                this.centerArray = this.pageArray;
            };
        },
        loadPreviousQuestions() {
            if (this.selectedPage != 1)
                this.loadQuestions(this.selectedPage - 1);
        },
        loadNextQuestions() {
            if (this.selectedPage != this.pageArray.length)
                this.loadQuestions(this.selectedPage + 1);
        },
        changeQuestionWishknowledgeState(questionId, isInWishknowledge) {
            for (var q in this.questions) {
                if (this.questions[q].Id == questionId) {
                    this.questions[q].IsInWishknowledge = isInWishknowledge;
                    break;
                }
            }
        },
        getUpdatedCorrectnessProbability(id) {
            var self = this;
            $.ajax({
                url: "/QuestionList/GetUpdatedCorrectnessProbability/",
                data: { questionId: id },
                type: "Post",
                success: result => {
                    var index = self.questions.findIndex((q) => { return q.Id == id });
                    if (index >= 0) {
                        self.questions[index].CorrectnessProbability = result.correctnessProbability;
                        self.questions[index].HasPersonalAnswer = result.hasPersonalAnswer;
                    }
                }
            });
        },
        addQuestionToList(q: QuestionListItem) {
            if (this.questions.length <= 0)
                this.renderNewSessionBar(q.Id);
            this.questions.push(q);
        },
        renderNewSessionBar(id) {
            $.ajax({
                url: "/QuestionList/RenderSessionHeaderWithQuestionId/",
                data: { questionId: id, categoryId: this.categoryId },
                type: "Post",
                success: data => {
                    $('#NoQuestionsSessionBar').replaceWith(data.html);
                }
            });
        }
    },
});
