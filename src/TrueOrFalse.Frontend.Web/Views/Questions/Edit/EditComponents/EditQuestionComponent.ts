////declare var SolutionType: {
////    Text: 0,
////    Numeric: 4,
////    Date: 6,
////    MultipleChoice_SingleSolution: 3,
////    MultipleChoice: 7,
////    Sequence: 5,
////    MatchList: 8,
////    FlashCard: 9,
////}

declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();



Vue.component('edit-question-component',
    {
        data() {
            return {
                id: 0,
                solutionType: null,
                textAnswer: null,
                singleSolutionAnswer: null,
                numericAnswer: null,
                sequenceAnswer: null,
                dateAnswer: null,
                multipleChoiceAnswer: null,
                matchListAnswer: null,
                flashCardSolution: null,
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
            solutionBuilder() {
                let solution = "";
                switch (this.solutionType) {
                    case 0: solution = this.textAnswer;
                        break;
                    case 3: solution = this.singleSolutionAnswer;
                        break;
                    case 4: solution = this.numericAnswer;
                        break;
                    case 5: solution = this.sequenceAnswer;
                        break;
                    case 6: solution = this.dateAnswer;
                        break;
                    case 7: solution = this.multipleChoiceAnswer;
                        break;
                    case 8: solution = this.matchListAnswer;
                        break;
                    case 9: solution = this.flashcardAnswer;
                        break;
                }

            },
        }
    });
