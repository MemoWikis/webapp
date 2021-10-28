declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('comment-component',
    {
        props: ['comment', 'questionId', 'currentUserImageUrl', 'currentUserName', 'isAdminString'],

        data: function () {
            return {
                readMore: false,
                showAnsweringPanel: false,
                addedAnswers: [''],
                isInstallationAdmin: this.isAdminString == 'True',
                isOwner: false,
                isLoggedIn: IsLoggedIn.Yes,
                showCommentAnswers: true,
                foldOut: false
            }
        },
        template: '#comment-component',

        created() {
            const self = this;
            self.isInstallationAdmin = self.isAdminString == "True";
            self.isOwner = self.comment.ImageUrl == self.currentUserImageUrl;
        },

        methods: {

            markAsSettled(commentId) {
                $.ajax({
                    type: 'POST',
                    url: "/AnswerComments/MarkCommentAsSettled",
                    data: { commentId: commentId },
                    cache: false,
                    success(e) {
                        eventBus.$emit('new-comment-added');
                    },
                    error(e) {
                        console.log(e);
                        window.alert("Ein Fehler ist aufgetreten");
                    }
                })
            },

            markAsUnsettled(commentId) {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: "/AnswerComments/MarkCommentAsUnsettled",
                    data: { commentId: commentId },
                    cache: false,
                    success(e) {
                        self.settled = false;
                        eventBus.$emit('new-comment-added');
                    },
                    error(e) {
                        console.log(e);
                        window.alert("Ein Fehler ist aufgetreten");
                    }
                });
            },
            emitSaveAnswer() {
                eventBus.$emit('saveAnswer');
            }
        }
    });


Vue.component('add-comment-component',
    {
        props: ['currentUserImageUrl', 'questionId', 'commentsLoaded'],
        data() {
            return {
                commentText: "",
                commentTitle: "",
                isLoggedIn: IsLoggedIn.Yes,
                commentEditor: null,
                commentJson: null,
                commentHtml: null,
                flashCardJson: null,
                highlightEmptyComment: false,
                highlightEmptyTitle: false

            }
        },

        template: '#add-comment-component',

        mounted() {
            this.initQuickCreate();
        },

        methods: {
            cancel() {
                eventBus.$emit('close-modal');
            },

            initQuickCreate() {
                var self = this;
                self.$nextTick(() => {

                    Vue.component('editor-content', tiptapEditorContent);
                    self.commentEditor = new tiptapEditor({
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
                                placeholder: 'Beschreibe hier dein Anliegen. Bitte höflich, freundlich und sachlich schreiben...',
                                showOnlyCurrent: true,
                            }),
                            tiptapImage
                        ],
                        onUpdate: ({ editor }) => {
                            self.commentJson = editor.getJSON();
                            self.commentText = editor.getHTML();
                        },
                    });
                });
            },



            saveComment() {
                if (this.commentText.length > 10 && this.commentTitle.length > 5) {
                    var self = this;
                    var params = {
                        questionId: self.questionId,
                        text: this.commentText,
                        title: this.commentTitle
                    };
                    $.ajax({
                        type: 'post',
                        contentType: "application/json",
                        url: "/AnswerComments/SaveComment",
                        data: JSON.stringify(params),
                        success: (result) => {
                            this.commentText = "";
                            this.commentEditor.commands.setContent('');
                            this.commentTitle = "";
                            this.highlightEmptyComment = false;
                            this.highlightEmptyTitle = false;
                            eventBus.$emit('new-comment-added');
                        },
                        error: () => { }
                    });
                } else {
                    this.highlightEmptyComment = false;
                    this.highlightEmptyTitle = false;

                    if (this.commentText.length < 10) {
                        this.highlightEmptyComment = true;
                    }
                    if (this.commentTitle.length < 5) {
                        this.highlightEmptyTitle = true;
                    }
                }
            },
            closeModal() {
                eventBus.$emit('close-modal');
            }
        }
    });


Vue.component('comment-answer-component',
    {
        props: ['answer', 'commentId', 'lastAnswer'],
        data() {
            return {
                id: parseInt(this.commentId),
                readMore: false
            }
        },
        template: '#comment-answer-component',


        mounted() {

        },

        methods: {

        }
    });

Vue.component('comment-answer-add-component',
    {
        props: ['currentUserImageUrl', 'parentCommentId', 'currentUserName', 'currentUserUrl'],
        data() {
            return {
                commentAnswerText: "",
                commentAnswerJson: "",
                isLoggedIn: IsLoggedIn.Yes,
                answerEditor: null,
                answerHtml: null,
                flashCardJson: null,
                highlightEmptyAnswer: false,
            }
        },

        template: '#comment-answer-add-component',

        created() {

            var self = this;
            eventBus.$on('saveAnswer', function () {
                self.saveCommentAnswer();
            });
        },

        mounted() {
            this.initQuickCreate();
        },

        methods: {

            initQuickCreate() {
                var self = this;
                self.$nextTick(() => {

                    Vue.component('editor-content', tiptapEditorContent);
                    self.answerEditor = new tiptapEditor({
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
                                placeholder: 'Bitte formuliere deinen Beitrag höflich, freundlich und sachlich.',
                                showOnlyCurrent: true,
                            }),
                            tiptapImage
                        ],
                        onUpdate: ({ editor }) => {
                            self.commentAnswerJson = editor.getJSON();
                            self.commentAnswerText = editor.getHTML();
                        },
                    });
                });
            },

            saveCommentAnswer() {
                if (this.commentAnswerText.length > 10) {
                    var self = this;
                    var params = {
                        commentId: this.parentCommentId,
                        text: this.commentAnswerText
                    };
                    $.ajax({
                        type: 'post',
                        contentType: "application/json",
                        url: "/AnswerComments/SaveAnswer",
                        data: JSON.stringify(params),
                        success: (result) => {
                            this.commentAnswerText = "";
                            this.answerEditor.commands.setContent('');
                            this.highlightEmptyAnswer = false;
                            eventBus.$emit('new-comment-added');
                        },
                        error: () => {}
                    });
                } else {
                   this.highlightEmptyAnswer = true;

                }
            },
        }
    });
