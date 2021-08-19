declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('comments-section-component',
    {
        props: [],
        data() {
            return {
                addedComments: [''],
                showSettledComments: false,
            }
        },

        created() {
            const self = this;
            eventBus.$on('addedComment', function (commentHTML) {
                self.addComment(commentHTML);
            });
        },

        methods: {

            addComment(commentHTML) {
                this.addedComments.push(commentHTML);
            },

            revealSettledComments(commentId) {
                $.ajax({
                    type: 'POST',
                    url: "/AnswerComments/GetAllAnswersInclSettledHtml",
                    data: { commentId: commentId },
                    cache: false,
                    success(data) {
                        this.settledComments = data;
                        console.log(data);
                    },
                    error(e) {
                        console.log(e);
                        window.alert("Ein Fehler ist aufgetreten.");
                    }
                });
            }
        }
    }),

    Vue.component('comment-answer-component',
        {
            props: ['idString', 'isSettledString'],
            data() {
                return {
                    id: parseInt(this.idString),
                    isSettled: this.isSettledString == 'True',
                    readMore: false
                }
            },

            mounted() {

            },

            methods: {

            }
        });



Vue.component('comment-component',
    {
        props: ['commentIdString', 'isAdminString', 'isOwnerString'],
        data() {
            return {
                readMore: false,
                showAnsweringPanel: false,
                settled: false,
                commentId: this.commentIdString,
                addedAnswers: [''],
                isInstallationAdmin: this.isAdminString == 'True',
                isOwner: this.isOwnerString == 'True',
            }
        },

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
Vue.component('comment-answer-add-component',
    {
        props: [],
        data() {
            return {
                commentAnswerText: "",
            }
        },

        mounted() {

        },

        methods: {
            saveCommentAnswer(parentCommentId) {
                var params = {
                    commentId: parentCommentId,
                    text: this.commentAnswerText
                };

                $.post("/AnswerComments/SaveAnswer", params, function (data) {
                    console.log(this.commentAnswerText);
                    eventBus.$emit('addedAnswer' + params.commentId, data);
                });
            }
        }
    });

Vue.component('add-comment-component',
    {
        data() {
            return {
                commentText: "",
            }
        },

        mounted() {

        },

        methods: {
            saveComment(currentQuestionId) {
                var params = {
                    questionId: currentQuestionId,
                    text: $("#txtNewComment").val()
                };
                $.post("/AnswerComments/SaveComment",
                    params,
                    function (data) {
                        this.commentText = "";
                        eventBus.$emit('addedComment', data);
                    });
            }
        }
    });
