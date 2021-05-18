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
        }
    });
