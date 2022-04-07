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
            sessionConfigJson: null
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

        eventBus.$on('tiptap-is-ready', () => {
            this.$nextTick(() => this.initEditor());
        });
        eventBus.$on('edit-question-is-ready', () => {
            this.editQuestionIsReady = true;
        });
        if (typeof (tiptapEditor) !== 'undefined' && tiptapEditor != null) {
            this.tiptapIsReady = true;
            this.initEditor();
        } else {
            Utils.LoadTiptap();
        }
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
                                    Alerts.showError({
                                        text: messages.error.image.tooBig
                                    });
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
            var lastIndex = $('.singleQuestionRow ').length > 0 ? parseInt($('#QuestionListComponent').attr("data-last-index")) + 1 : null;

            var sessionConfigJsonString = localStorage.getItem('sessionConfigJson');
            if (sessionConfigJsonString != null)
                this.sessionConfigJson = JSON.parse(sessionConfigJsonString);

            var json = {
                CategoryId: this.currentCategoryId,
                TextHtml: this.questionHtml,
                Answer: this.flashCardAnswer,
                Visibility: this.isPrivate ? 1 : 0,
                AddToWishknowledge: this.addToWishknowledge,
                LastIndex: lastIndex,
                SessionConfig: this.sessionConfigJson
            }
            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/Question/CreateFlashcard',
                data: JSON.stringify(json),
                success: (data) => {
                    var answerBody = new AnswerBody();
                    var skipIndex = this.questions != null ? -5 : 0;

                    answerBody.Loader.loadNewQuestion("/AnswerQuestion/RenderAnswerBodyByLearningSession/" +
                        "?skipStepIdx=" +
                        skipIndex +
                        "&index=" +
                        lastIndex);

                    if (data.SessionIndex < 0)
                        Alerts.showSuccess({
                            text: self.edit ? messages.success.question.saved : messages.success.question.created,
                            customHtml: '<div class="session-config-error fade in col-xs-12"><span><b>Der Fragenfilter ist aktiv.</b> Die Frage wird dir nicht angezeigt. Setze den Filter zurück, um alle Fragen anzuzeigen.</span></div>',
                            customBtn: '<div class="btn memo-button col-xs-4 btn-link" data-dismiss="modal" onclick="eventBus.$emit(\'reset-session-config\')">Filter zurücksetzen</div>',
                        });

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