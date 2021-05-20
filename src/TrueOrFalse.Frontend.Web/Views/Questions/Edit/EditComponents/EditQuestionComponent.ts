declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('edit-question-component',
    {
        data() {
            return {
                id: 0,
                solutionType: null,
                textJson: null,
                singleJsonJson: null,
                numericJson: null,
                sequenceJson: null,
                dateJson: null,
                multipleChoiceJson: null,
                matchListJson: null,
                flashCardJson: null,
            }
        },
        mounted() {
            $('#AddCategoryModal').on('show.bs.modal',
                event => {
                    this.id = $('#AddCategoryModal').data('question').id;
                    this.solutionType = $('#AddCategoryModal').data('question').type;
                });
        },
        watch: {
        },
        methods: {
            getSolution() {
                let solution = "";
                switch (this.solutionType) {
                    case SolutionType.Text: solution = this.textJson;
                        break;
                    case SolutionType.MultipleChoice_SingleSolution: solution = this.singleJsonJson;
                        break;
                    case SolutionType.Numeric: solution = this.numericJson;
                        break;
                    case SolutionType.Sequence: solution = this.sequenceJson;
                        break;
                    case SolutionType.Date: solution = this.dateJson;
                        break;
                    case SolutionType.MultipleChoice: solution = this.multipleChoiceJson;
                        break;
                    case SolutionType.MatchList: solution = this.matchListJson;
                        break;
                    case SolutionType.FlashCard: solution = this.flashCardJson;
                        break;
                }

                return solution;
            },
            sumbitQuestion() {
                var lastIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
                var url;
                if (this.createQuestion)
                    url = '/EditQuestion/CreateQuestion';
                else
                    url = '/EditQuestion/EditQuestion';
                let solution = this.getSolution();
                var questionJson = {
                    Text: this.questionHtml,
                    Answer: solution,
                    Visibility: this.visibility,
                    AddToWishknowledge: this.addToWishknowledge,
                    CategoryIds: this.selectedCategoryIds,
                }
                var json = {
                    CategoryId: this.currentCategoryId,
                    Text: this.questionHtml,
                    Answer: this.answerHtml,
                    Visibility: this.visibility,
                    AddToWishknowledge: this.addToWishknowledge,
                    LastIndex: lastIndex,
                }
                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: '/QuestionList/CreateFlashcard',
                    data: JSON.stringify(json),
                    success: function (data) {
                        var answerBody = new AnswerBody();
                        var skipIndex = this.questions != null ? -5 : 0;

                        answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                            "?skipStepIdx=" +
                            skipIndex +
                            "&index=" +
                            lastIndex);
                        eventBus.$emit('add-question-to-list', data.Data);
                        eventBus.$emit("change-active-question", lastIndex);
                    },
                });
            },

        }
    });
