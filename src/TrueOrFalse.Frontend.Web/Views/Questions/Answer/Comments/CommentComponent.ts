declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();


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
                    parentContainer.append(data);
                    console.log(data);
                });
            }
        }
    });

Vue.component('add-comment-component',
    {
        data() {
            return {
            }
        },

        mounted() {

        },

        methods: {
        }
    });
