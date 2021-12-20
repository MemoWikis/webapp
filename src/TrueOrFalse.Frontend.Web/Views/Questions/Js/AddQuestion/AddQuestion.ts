Vue.component('add-question-component', {
    props: ['current-category-id'],
    data() {
        return {
            highlightEmptyFields: false,
            isLoggedIn: IsLoggedIn.Yes,
            addToWishknowledge: true,
            questionEditor: null,
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
            isPrivate: true,
            tiptapIsReady: false,
            editQuestionIsReady: false,
        }
    },

    watch: {
        isPrivate() {
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
        if (this.isPrivate)
            this.licenseIsValid = true;
        if (typeof (tiptapEditor) !== 'undefined' && tiptapEditor != null) {
            this.tiptapIsReady = true;
            this.initEditor();
        } else {
            this.loadTiptap();
        }
        eventBus.$on('tiptap-is-ready', () => {
            this.initEditor();
        });
        eventBus.$on('edit-question-is-ready', () => {
            this.editQuestionIsReady = true;
        });
    },

    methods: {
        initQuickCreate() {
            var self = this;
            self.$nextTick(() => {

                Vue.component('editor-content', tiptapEditorContent);
                self.questionEditor = new tiptapEditor({
                    editable: true,
                    extensions: [
                        tiptapStarterKit,
                        tiptapLink.configure({
                            HTMLAttributes: {
                                target: '_self',
                                rel: 'noopener noreferrer nofollow'
                            }
                        }),
                        tiptapCodeBlockLowlight.configure({
                            lowlight,
                        }),
                        tiptapUnderline,
                        tiptapPlaceholder.configure({
                            emptyEditorClass: 'is-editor-empty',
                            emptyNodeClass: 'is-empty',
                            placeholder: 'Gib den Fragetext ein',
                            showOnlyCurrent: true,
                        }),
                        tiptapImage
                    ],
                    editorProps: {
                        handlePaste: (view, pos, event) => {
                            let eventContent = event.content.content;
                            if (eventContent.length >= 1 && !_.isEmpty(eventContent[0].attrs)) {
                                let src = eventContent[0].attrs.src;
                                if (src.length > 1048576 && src.startsWith('data:image')) {
                                    let data = {
                                        msg: messages.error.image.tooBig
                                    }
                                    eventBus.$emit('show-error', data);
                                    return true;
                                }
                            }
                        },
                    },
                    onUpdate: ({ editor }) => {
                        self.questionJson = editor.getJSON();
                        self.questionHtml = editor.getHTML();
                        self.formValidator();
                    },
                });
            });
        },
        initEditor() {
            if (this.questionEditor != null)
                return;
            if (this.editQuestionIsReady) {
                this.initQuickCreate();
            } else {
                var self = this;
                $.ajax({
                    type: 'get',
                    url: '/EditQuestion/GetEditQuestionModal/',
                    success: function (html) {
                        $(html).insertBefore('#EditQuestionLoaderApp');
                        self.$nextTick(() => {
                            self.tiptapIsReady = true;
                            self.initQuickCreate();
                        });
                    },
                });
            }
        },
        loadTiptap() {
            $.ajax({
                type: 'get',
                url: '/EditCategory/GetTiptap/',
                success: function (html) {
                    $(html).insertAfter('script#pin-category-template');
                },
            });
        },
        addFlashcard() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("CreateFlashCard");
                return;
            }
            if (this.disabled) {
                this.highlightEmptyFields = true;
                return;
            }
            var self = this;
            var lastIndex = parseInt($('#QuestionListComponent').attr("data-last-index")) + 1;
            var json = {
                CategoryId: this.currentCategoryId,
                TextHtml: this.questionHtml,
                Answer: this.flashCardAnswer,
                Visibility: this.isPrivate ? 1 : 0,
                AddToWishknowledge: this.addToWishknowledge,
                LastIndex: lastIndex,
            }
            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/Question/CreateFlashcard',
                data: JSON.stringify(json),
                success: function (data) {
                    var answerBody = new AnswerBody();
                    var skipIndex = this.questions != null ? -5 : 0;

                    answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                        "?skipStepIdx=" +
                        skipIndex +
                        "&index=" +
                        lastIndex);

                    eventBus.$emit('add-question-to-list', data);
                    eventBus.$emit("change-active-question", lastIndex);
                    self.highlightEmptyFields = false;
                    self.questionEditor.commands.setContent('');
                    self.questionHtml = '';
                    self.flashCardAnswer = '';
                    eventBus.$emit('clear-flashcard');
                    let headerCount = parseInt($('#CategoryHeaderQuestionCount').text());
                    $('#CategoryHeaderQuestionCount').text(++headerCount);
                    headerCount != 1
                        ? $('#CategoryHeaderQuestionCountLabel').text('Fragen')
                        : $('#CategoryHeaderQuestionCountLabel').text('Frage');
                },
            });
        },
        createQuestion() {
            var question = {
                categoryId: parseInt(this.currentCategoryId),
                edit: false,
                questionHtml: this.questionHtml,
                solution: this.flashCardAnswer,
            };
            eventBus.$emit('open-edit-question-modal', question);
            this.questionEditor.commands.setContent('');
            eventBus.$emit('clear-flashcard');
        },
        formValidator() {
            var questionIsValid = this.questionEditor.state.doc.textContent.length > 0;
            var solutionIsValid = this.solutionIsValid;
            this.licenseIsValid = this.licenseConfirmation || this.isPrivate;

            this.disabled = !questionIsValid || !solutionIsValid || !this.licenseIsValid;
        }
    }
})