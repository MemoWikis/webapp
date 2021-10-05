declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('comment-component',
    {
        props: ['comment', 'questionId', 'currentUserImageUrl'],

        data: function () {
            return {
                readMore: false,
                showAnsweringPanel: false,
                settled: false,
                addedAnswers: [''],
                isInstallationAdmin: this.isAdminString == 'True',
                isOwner: false,
                isLoggedIn: IsLoggedIn.Yes,
                showCommentAnswers: false
            }
        },
        template: '#comment-component',

        created() {
            const self = this;
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
                isLoggedIn: IsLoggedIn.Yes,
        }
        },

        template: '#add-comment-component',

        mounted() {

        },

        methods: {
            saveComment() {
                var self = this;
                var params = {
                    questionId: self.questionId,
                    text: this.commentText
                };
                $.post("/AnswerComments/SaveComment",
                    params,
                    () => {
                        this.commentText = "";
                        eventBus.$emit('new-comment-added');
                    });
            },
            closeModal() {
                eventBus.$emit('close-modal');
            }
        }
    });


Vue.component('comment-answer-component',
    {
        props: ['answer', 'commentId'],
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
        props: ['currentUserImageUrl', 'parentCommentId'],
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
