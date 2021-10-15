declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('comment-component',
    {
        props: ['comment', 'questionId', 'currentUserImageUrl', 'currentUserName'],

        data: function () {
            return {
                readMore: false,
                showAnsweringPanel: false,
                settled: false,
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
            self.foldOut = !self.comment.IsSettled;
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
                })
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
                highlightEmptyFields: false,

            }
        },

        template: '#add-comment-component',

        mounted() {
            this.initQuickCreate();
        },

        methods: {

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
                if (this.commentText.length > 20 && this.commentTitle.length > 5) {
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
                            eventBus.$emit('new-comment-added');
                        },
                        error: () => { }
                    });
                } else {
                    console.log("Kommentar zu kurz");
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
            }
        },

        template: '#comment-answer-add-component',

        mounted() {

        },

        methods: {
            saveCommentAnswer() {
                var params = {
                    commentId: this.parentCommentId,
                    text: this.commentAnswerText
                };

                $.post("/AnswerComments/SaveAnswer", params, () => {
                    this.commentAnswerText = "";
                    eventBus.$emit('new-comment-added');
                });
            }
        }
    });
