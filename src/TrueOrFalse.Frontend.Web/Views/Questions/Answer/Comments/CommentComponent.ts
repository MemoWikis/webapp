declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

class CommentModel {
    Id: Number;
    CreatorName : String;
    CreationDate: String;
    CreationDateNiceText: String;
    ImageUrl: String;
    Text: String;


}
Vue.component('comments-section-component',
    {
        props: [],
        data() {
            return {
                
                addedComments: CommentModel,
                showSettledComments: false,
            }
        },

        mounted() {
            eventBus.$on('addedComment', function (commentHTML) {

            });
        },

        methods: {
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
        props: [],
        data() {
            return {
                readMore: false,
                showAnsweringPanel: false,
                settled: false,
            }
        },

        mounted() {

        },

        methods: {
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

                var parentContainer = $($(this.el).parents(".panel")[0]);
                $.post("/AnswerComments/SaveAnswer", params, function (data) {
                    console.log(data);

                    parentContainer.append(data);
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
                        console.log('"'+data+'"');
                        this.commentText = "";
                        eventBus.$emit('addedComment', data);
                        $("#comments").append(data);
                    });
            }
        }
    });
