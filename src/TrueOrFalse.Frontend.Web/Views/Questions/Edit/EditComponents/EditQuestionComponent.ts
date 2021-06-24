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
                solutionType: null,
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
                empty: 'empty',
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
                    editorProps: {
                        handleClick: (view, pos, event) => {
                        },
                        attributes: {
                            id: 'QuestionInputField',
                        }
                    },
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
                            emptyNodeText: 'Erklärungen, Zusatzinfos, Merkhilfen, Abbildungen, weiterführende Literatur und Links etc.',
                            showOnlyCurrent: true,
                        })
                    ],
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.descriptionHtml = getHTML();
                    },
                }),
                descriptionHtml: null,
                visibility: 1,
                categoryIds: [],
                categories: [],
                searchTerm: '',
                lockDropdown: true,
                debounceSearchCategory: _.debounce(this.searchCategory, 300),
                selectedCategories: [],
                totalCount: 0,
                showDropdown: false,
                licenseId: 0,
                sessionIndex: 0,
                solutionMetadataJson: null,
                licenseConfirmation: false,
                isLearningSession: false,
                currentLearningSessionIndex: null,
                solutionIsValid: false,
                showMore: false,
                disabled: true,
                highlightEmptyField: false,
            }
        },
        mounted() {
            //eventBus.$on('show-missing-fields',
            //    () => {
            //        if (this.questionEditor.state.doc.textContent.length === 0)
            //            this.highlightEmptyField = true;
            //    });
            eventBus.$on('open-edit-question-modal',
                e => {
                    var question = {
                        questionId: e.questionId,
                        edit: e.edit,
                        sessionIndex: e.sessionIndex,
                        categoryId: e.categoryId
                    };
                    $('#EditQuestionModal').data('question', question).modal('show');
                });
            $('#EditQuestionModal').on('show.bs.modal',
                event => {
                    this.id = $('#EditQuestionModal').data('question').questionId;
                    if ($('#EditQuestionModal').data('question').edit) {
                        this.edit = true;
                        this.getQuestionData(this.id);
                        if ($('#hddIsLearningSession').length > 0)
                            this.isLearningSession = true;
                        this.currentLearningSessionIndex = $('#hddIsLearningSession').attr('data-current-step-idx');
                        if ($('#EditQuestionModal').data('question').sessionIndex)
                            this.sessionIndex = $('#EditQuestionModal').data('question').sessionIndex;
                        else if (this.currentLearningSessionIndex)
                            this.sessionIndex = this.currentLearningSessionIndex;
                    } else {
                        this.solutionType = 1;
                        let categoryId = $('#EditQuestionModal').data('question').categoryId;
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
                        this.sessionIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
                    }
                });

            $('#EditQuestionModal').on('hidden.bs.modal', () => {
                this.questionEditor.destroy();
                this.descriptionEditor.destroy();
                Object.assign(this.$data, this.$options.data.apply(this));
            });
        },
        watch: {
            highlightEmptyField(val) {
                if (val)
                    $('#QuestionInputField').addClass('is-empty');
                else
                    $('#QuestionInputField').removeClass('is-empty');
            },
            searchTerm(term) {
                if (term.length > 0 && this.lockDropdown == false) {
                    this.showDropdown = true;
                    this.debounceSearchCategory();
                }
                else
                    this.showDropdown = false;
            },
            questionHtml() {
                this.formValidator();
                if (this.questionEditor.state.doc.textContent.length > 0)
                    this.highlightEmptyField = false;
                else
                    this.highlightEmptyField = true;
            },
            solutionType() {
                this.solutionIsValid = false;
                this.solutionMetadataJson = null;
            },

            solutionIsValid() {
                this.formValidator();
            },
            visibility(val) {
                this.formValidator();
            },
            licenseConfirmation() {
                this.formValidator();
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
                        self.initiateSolution(data.Solution);
                        self.questionHtml = data.Text;
                        self.questionEditor.setContent(data.Text);
                        self.categoryIds = data.CategoryIds;
                        self.descriptionHtml = data.DescriptionHtml;
                        self.selectedCategories = data.Categories;
                        self.licenseId = data.LicenseId;
                        self.solutionMetadataJson = data.SolutionMetadataJson;
                        self.visibility = data.Visibility;
                    },
                });
            },
            getSolution() {
                let solution = "";
                let solutionType = parseInt(this.solutionType);
                switch (solutionType) {
                    case SolutionType.Text: return this.textSolution;
                    case SolutionType.MultipleChoice: solution = this.multipleChoiceJson;
                        break;
                    case SolutionType.MatchList: solution = this.matchListJson;
                        break;
                    case SolutionType.FlashCard: return this.flashCardAnswer;
                }
                
                return JSON.stringify(solution);
            },
            initiateSolution(solution) {
                let solutionType = parseInt(this.solutionType);
                switch (solutionType) {
                    case SolutionType.Text:
                        this.textSolution = solution;
                    break;
                    case SolutionType.MultipleChoice:
                        this.multipleChoiceJson = solution;
                    break;
                    case SolutionType.MatchList:
                        this.matchListJson = solution;
                    break;
                    case SolutionType.FlashCard:
                        this.flashCardJson = solution;
                }

                return solution;
            },
            save() {
                var url = this.edit ? '/Question/Edit' : '/Question/Create';
                var json = this.getSaveJson();
                var self = this;

                if (this.disabled) {
                    eventBus.$emit('show-missing-fields');
                    return;
                }

                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: url,
                    data: JSON.stringify(json),
                    success: function (result) {
                        if (self.isLearningSession) {
                            var answerBody = new AnswerBody();
                            var skipIndex = this.questions != null ? -5 : 0;
                            answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                                "?skipStepIdx=" +
                                skipIndex +
                                "&index=" +
                                self.sessionIndex);

                            eventBus.$emit('reload-question-id', result.Data.Id);
                            eventBus.$emit("change-active-question", self.sessionIndex);
                        }
                        else
                            self.reloadAnswerBody(result.Data.Id);

                        $('#EditQuestionModal').modal('hide');
                    },
                });
            },

            reloadAnswerBody(id) {
                $.ajax({
                    type: 'post',
                    contentType: "application/json",
                    url: '/AnswerQuestion/RenderAnswerBody',
                    data: JSON.stringify({ questionId: id }),
                    success: function (html) {
                        $('#AnswerBody').replaceWith(html);
                        var answerBody = new AnswerBody();
                    },
                });
            },

            getSaveJson() {
                let solution = this.getSolution();
                let solutionType = parseInt(this.solutionType);
                if (this.solutionType == 4 || this.solutionType == 7)
                    solutionType = 1;

                let licenseId = this.getLicenseId();

                var editJson = {
                    QuestionId: this.id,
                }
                var createJson = {
                    AddToWishknowledge: this.addToWishknowledge,
                }

                var jsonExtension = {
                    CategoryIds: this.categoryIds,
                    Text: this.questionHtml,
                    Solution: solution,
                    SolutionType: solutionType,
                    Visibility: this.visibility,
                    SolutionMetadataJson: this.solutionMetadataJson,
                    LicenseId: licenseId,
                    SessionIndex: this.sessionIndex,
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
            },
            formValidator() {
                var questionIsValid = this.questionHtml != null && this.questionHtml.length > 0;
                var solutionIsValid = this.solutionIsValid;
                var licenseIsValid = this.licenseConfirmation || this.visibility == 1;

                this.disabled = !questionIsValid || !solutionIsValid || !licenseIsValid;
            }
        }
    });
