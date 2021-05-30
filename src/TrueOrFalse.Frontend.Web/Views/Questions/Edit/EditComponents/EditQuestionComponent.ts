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

function initialState() {
}


Vue.component('editor-menu-bar', tiptap.EditorMenuBar);
Vue.component('editor-content', tiptap.EditorContent);
var editQuestionComponent = Vue.component('edit-question-component',
    {
        data() {
            return {
                id: null,
                addToWuwi: false,
                solutionType: 1,
                textSolution: null,
                singleSolutionJson: null,
                numericSolution: null,
                sequenceJson: null,
                dateSolution: null,
                multipleChoiceJson: null,
                matchListJson: null,
                flashCardJson: null,
                flashCardAnswer: null,
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
                descriptionEditor: new tiptap.Editor({
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
                        new tiptapExtensions.CodeBlockHighlight({
                            languages: {
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
                            },
                        }),
                        new tiptapExtensions.Placeholder({
                            emptyEditorClass: 'is-editor-empty',
                            emptyNodeClass: 'is-empty',
                            emptyNodeText: 'Gib den Fragetext ein',
                            showOnlyCurrent: true,
                        })
                    ],
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.descriptionHtml = getHTML();
                    },
                }),
                descriptionHtml: null,
                visibility: 0,
                categoryIds: [],
                categories: [],
                searchTerm: '',
                lockDropdown: true,
                debounceSearchCategory: _.debounce(this.searchCategory, 300),
                selectedCategories: [],
                totalCount: 0,
                showDropdown: false,
                licenseId: 0,
                lastIndex: 0,
                solutionMetadataJson: null,
            }
        },
        mounted() {
            $('#EditQuestionModal').on('show.bs.modal',
                event => {
                    this.solutionType = 1;
                    this.id = $('#EditQuestionModal').data('question').questionId;
                    if ($('#EditQuestionModal').data('question').edit) {
                        this.edit = true;
                        this.getQuestionData(this.id);
                    } else {
                        let categoryId = $('#EditQuestionModal').data('question').categoryId
                        this.categoryIds.push(categoryId);
                        var json = { categoryId };
                        var self = this;
                        $.ajax({
                            type: 'post',
                            contentType: "application/json",
                            url: '/Category/GetMiniCategoryItem',
                            data: JSON.stringify(json),
                            success: function (data) {
                                self.selectedCategories.push(data.Category);
                            },
                        });
                        this.edit = false;
                    }
                });

            $('#EditQuestionModal').on('hidden.bs.modal', () => {
                this.questionEditor.destroy();
                this.descriptionEditor.destroy();
                Object.assign(this.$data, this.$options.data.apply(this));
            });
        },
        watch: {
            searchTerm(term) {
                if (term.length > 0 && this.lockDropdown == false) {
                    this.showDropdown = true;
                    this.debounceSearchCategory();
                }
                else
                    this.showDropdown = false;
            },

            solutionType() {
                this.solutionMetadataJson = null;
            }
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
                        self.questionHtml = data.Text;
                        self.questionEditor.setContent(data.Text);
                        self.categoryIds = data.CategoryIds;
                        self.descriptionHtml = data.DescriptionHtml;
                        self.selectedCategories = data.Categories;
                        self.licenseId = data.LicenseId;
                        self.solutionMetadataJson = data.SolutionMetadataJson;
                    },
                });
            },
            getSolution() {
                let solution = "";
                switch (this.solutionType) {
                    case SolutionType.Text: solution = this.textSolution;
                        break;
                    case SolutionType.MultipleChoice_SingleSolution: solution = this.singleSolutionJson;
                        break;
                    case SolutionType.Numeric: solution = this.numericSolution;
                        break;
                    case SolutionType.Sequence: solution = this.sequenceJson;
                        break;
                    case SolutionType.Date: solution = this.dateSolution;
                        break;
                    case SolutionType.MultipleChoice: solution = this.multipleChoiceJson;
                        break;
                    case SolutionType.MatchList: solution = this.matchListJson;
                        break;
                    case SolutionType.FlashCard: return this.flashCardAnswer;
                }
                
                return JSON.stringify(solution);
            },
            initiateSolution(solutionType, solution) {
                switch (this.solutionType) {
                    case SolutionType.Text:
                        this.textSolution = solution;
                    break;
                    case SolutionType.MultipleChoice_SingleSolution:
                        this.singleSolutionJson = solution;
                    break;
                    case SolutionType.Numeric:
                        this.numericSolution = solution;
                    break;
                    case SolutionType.Sequence:
                        this.sequenceJson = solution;
                    break;
                    case SolutionType.Date:
                        this.dateSolution = solution;
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
            save() {
                this.lastIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
                var url;
                if (this.edit)
                    url = '/Question/Edit';
                else
                    url = '/Question/Create';

                var json = this.getSaveJson();

                var self = this;
                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: url,
                    data: JSON.stringify(json),
                    success: function (data) {
                        var answerBody = new AnswerBody();
                        var skipIndex = this.questions != null ? -5 : 0;

                        answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                            "?skipStepIdx=" +
                            skipIndex +
                            "&index=" +
                            self.lastIndex);
                        eventBus.$emit('add-question-to-list', data.Data);
                        eventBus.$emit("change-active-question", self.lastIndex);
                    },
                });
            },

            getSaveJson() {
                let solution = this.getSolution();
                let solutionType = this.solutionType;
                if (this.solutionType == 4 || this.solutionType == 7)
                    solutionType = 1;
                let licenseId = this.getLicenseId();

                var editJson = {
                    CategoryIds: this.categoryIds,
                    QuestionId: this.id,
                }
                var createJson = {
                    CategoryIds: [this.currentCategoryId],
                    AddToWishknowledge: this.addToWishknowledge,
                    LastIndex: this.lastIndex,
                }

                var jsonExtension = {
                    Text: this.questionHtml,
                    Solution: solution,
                    SolutionType: solutionType,
                    Visibility: this.visibility,
                    SolutionMetadataJson: this.solutionMetadataJson,
                    LicenseId: licenseId
                }
                var json = this.edit ? editJson : createJson;

                $.extend(json, jsonExtension);

                return json;
            },

            searchCategory() {
                this.showDropdown = true;
                var self = this;
                var data = {
                    term: self.searchTerm,
                    type: 'Categories'
                };

                $.get("/Api/Search/ByNameForVue", data,
                    function (result) {
                        self.categories = result.categories;
                        self.totalCount = result.totalCount;
                        self.$nextTick(() => {
                            $('[data-toggle="tooltip"]').tooltip();
                        });
                    });
            },

            getLicenseId() {
                if (this.licenseId == 0) {
                    if (this.visibility == 1)
                        return 0;
                    else return 1;
                }
                return this.licenseId;
            },

            selectCategory(category) {
                this.showDropdown = false;
                this.lockDropdown = true;
                this.searchTerm = '';

                var index = this.categoryIds.indexOf(category.Id);
                if (index < 0) {
                    this.categoryIds.push(category.Id);
                    this.selectedCategories.push(category);
                }
            },

            removeCategory(data) {
                if (this.selectedCategories.length > 1) {
                    this.selectedCategories.splice(data.index, 1);
                    var categoryIdIndex = this.categoryIds.indexOf(data.categoryId);
                    this.categoryIds.splice(categoryIdIndex, 1);
                }
            }

        }
    });
