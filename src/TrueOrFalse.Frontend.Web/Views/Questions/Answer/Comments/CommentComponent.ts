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
                isOwner: this.comment.imageUrl == this.currentUserImageUrl,
                isLoggedIn: IsLoggedIn.Yes,
                showCommentAnswers: false
            }
        },
        template: '#comment-component',

        created() {
            const self = this;
            eventBus.$on('addedAnswer' + self.commentId, function (answerHTML) {
                console.log('addedAnswerCommentComp' + self.commentId);
                self.addAnswer(answerHTML);
            });
        },

        methods: {
            addAnswer(answerHTML) {
                this.addedAnswers.push(answerHTML);
            },

            markAsSettled(commentId) {
                $.ajax({
                    type: 'POST',
                    url: "/AnswerComments/MarkCommentAsSettled",
                    data: { commentId: commentId },
                    cache: false,
                    success(e) {
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
        props: ['currentUserImageUrl', 'questionId'],
        data() {
            return {
                commentText: "",
                isLoggedIn: IsLoggedIn.Yes
            }
        },

        template: '#add-comment-component',

        mounted() {

        },

        methods: {
            saveComment() {
                var self = this;
                console.log(self.questionId);
                var params = {
                    questionId: self.questionId,
                    text: $("#txtNewComment").val()
                };
                $.post("/AnswerComments/SaveComment",
                    params,
                    function (data) {
                        this.commentText = "";
                        eventBus.$emit('addedComment', data);
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

                $.post("/AnswerComments/SaveAnswer", params, function (data) {
                    console.log(this.commentAnswerText);
                    eventBus.$emit('addedAnswer' + params.commentId, data);
                });
            }
        }
    });
