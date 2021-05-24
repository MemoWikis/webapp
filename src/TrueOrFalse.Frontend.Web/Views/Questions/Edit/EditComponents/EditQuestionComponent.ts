declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions,
} = tiptapBuild;
var {
    apache,
    //cLike,
    xml,
    bash,
    //c,
    coffeescript,
    csharp,
    css,
    markdown,
    diff,
    ruby,
    go,
    http,
    ini,
    java,
    javascript,
    json,
    kotlin,
    less,
    lua,
    makefile,
    perl,
    nginx,
    objectivec,
    php,
    phpTemplate,
    plaintext,
    properties,
    python,
    pythonREPL,
    rust,
    scss,
    shell,
    sql,
    swift,
    yaml,
    typescript,
} = hljsBuild;


Vue.component('editor-menu-bar', tiptap.EditorMenuBar);
Vue.component('editor-content', tiptap.EditorContent);
Vue.component('edit-question-component',
    {
        data() {
            return {
                id: 0,
                addToWuwi: false,
                solutionType: 1,
                textJson: null,
                singleSolutionJson: null,
                numericJson: null,
                sequenceJson: null,
                dateJson: null,
                multipleChoiceJson: null,
                matchListJson: null,
                flashCardJson: null,
                edit: false,
                questionEditor: new tiptap.Editor({
                    editable: true,
                    extensions: [
                        new tiptapExtensions.Blockquote(),
                        new tiptapExtensions.BulletList(),
                        new tiptapExtensions.CodeBlock(),
                        new tiptapExtensions.HardBreak(),
                        new tiptapExtensions.ListItem(),
                        new tiptapExtensions.OrderedList(),
                        new tiptapExtensions.TodoItem(),
                        new tiptapExtensions.TodoList(),
                        new tiptapExtensions.Link(),
                        new tiptapExtensions.Bold(),
                        new tiptapExtensions.Code(),
                        new tiptapExtensions.Italic(),
                        new tiptapExtensions.Strike(),
                        new tiptapExtensions.Underline(),
                        new tiptapExtensions.History(),
                        //new tiptapExtensions.CodeBlockHighlight({
                        //    languages: {
                        //        apache,
                        //        //cLike,
                        //        xml,
                        //        bash,
                        //        //c,
                        //        coffeescript,
                        //        csharp,
                        //        css,
                        //        markdown,
                        //        diff,
                        //        ruby,
                        //        go,
                        //        http,
                        //        ini,
                        //        java,
                        //        javascript,
                        //        json,
                        //        kotlin,
                        //        less,
                        //        lua,
                        //        makefile,
                        //        perl,
                        //        nginx,
                        //        objectivec,
                        //        php,
                        //        phpTemplate,
                        //        plaintext,
                        //        properties,
                        //        python,
                        //        pythonREPL,
                        //        rust,
                        //        scss,
                        //        shell,
                        //        sql,
                        //        swift,
                        //        yaml,
                        //        typescript,
                        //    },
                        //}),
                        new tiptapExtensions.Placeholder({
                            emptyEditorClass: 'is-editor-empty',
                            emptyNodeClass: 'is-empty',
                            emptyNodeText: 'Gib den Fragetext ein',
                            showOnlyCurrent: true,
                        })
                    ],
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.questionJson = getJSON();
                        this.questionHtml = getHTML();
                    },
                }),
                question: null,
                questionJson: null,
                questionHtml: null,
                visibility: 0,
            }
        },
        mounted() {
            $('#EditQuestionModal').on('show.bs.modal',
                event => {
                    this.solutionType = null;
                    this.id = $('#EditQuestionModal').data('question').id;
                    if ($('#EditQuestionModal').data('question').edit) {
                        this.edit = true;
                        this.getQuestionData(this.id);
                    } else {
                        this.edit = false;
                    }
                });
        },
        watch: {
        },
        methods: {
            getQuestionData(id) {
                var json = { questionId: id };
                var self = this;
                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: '/Questions/GetQuestionData',
                    data: JSON.stringify(json),
                    success: function (data) {
                        self.solutionType = data.SolutionType;
                        self.initiateSolution(data.SolutionType, data.Solution);
                    },
                });
            },
            getSolution() {
                let solution = "";
                switch (this.solutionType) {
                    case SolutionType.Text: solution = this.textJson;
                        break;
                    case SolutionType.MultipleChoice_SingleSolution: solution = this.singleSolutionJson;
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
            initiateSolution(solutionType, solution) {
                switch (this.solutionType) {
                    case SolutionType.Text:
                        this.textJson = solution;
                    break;
                    case SolutionType.MultipleChoice_SingleSolution:
                        this.singleSolutionJson = solution;
                    break;
                    case SolutionType.Numeric:
                        this.numericJson = solution;
                    break;
                    case SolutionType.Sequence:
                        this.sequenceJson = solution;
                    break;
                    case SolutionType.Date:
                        this.dateJson = solution;
                    break;
                    case SolutionType.MultipleChoice:
                        this.multipleChoiceJson = solution;
                    break;
                    case SolutionType.MatchList:
                        this.matchListJson = solution;
                    break;
                    case SolutionType.FlashCard:
                        this.flashCardJson = solution;
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
