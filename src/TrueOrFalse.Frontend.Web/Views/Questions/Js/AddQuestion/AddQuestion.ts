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

Vue.component('add-question-component', {
    props: ['current-category-id'],
    data() {
        return {
            highlightEmptyFields: false,
            isLoggedIn: IsLoggedIn.Yes,
            visibility: 1,
            addToWishknowledge: true,
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
                    this.formValidator();
                },
            }),
            question: null,
            questionJson: null,
            questionHtml: null,
            solutionType: 9,
            flashCardAnswer: null,
            flashCardJson: null,
            licenseConfirmation: false,
            showMore: false,
            licenseIsValid: false,
            solutionIsValid: false,
            disabled: true,
        }
    },

    watch: {
        visibility() {
            this.formValidator();
        },
        licenseConfirmation() {
            this.formValidator();
        },
        flashCardAnswer() {
            this.formValidator();
        }
    },

    mounted() {
        if (this.visibility == 1)
            this.licenseIsValid = true;
    },

    methods: {
        addFlashcard() {
            if (this.disabled) {
                this.highlightEmptyFields = true;
                return;
            }
            var self = this;
            var lastIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
            var json = {
                CategoryId: this.currentCategoryId,
                Text: this.questionHtml,
                Answer: this.flashCardAnswer,
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
                    self.highlightEmptyFields = false;
                    self.questionEditor.setContent('');
                    eventBus.$emit('clear-flashcard');
                },
            });
        },
        createQuestion() {
            var question = {
                categoryId: parseInt(this.currentCategoryId),
                edit: false,
            };
            eventBus.$emit('open-edit-question-modal', question);
        },
        formValidator() {
            var questionIsValid = this.questionEditor.state.doc.textContent.length > 0;
            var solutionIsValid = this.solutionIsValid;
            this.licenseIsValid = this.licenseConfirmation || this.visibility == 1;

            this.disabled = !questionIsValid || !solutionIsValid || !this.licenseIsValid;
        }
    }
})