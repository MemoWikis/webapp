interface Comments {
    id: number,
    creatorName: string,
    creationDate: string,
    creationDateNiceText: string,
    creatorImageUrl: string,
    creatorUrl: string,
    text: string,
    shouldBeImproved: boolean,
    shouldBeDeleted: boolean,
    isSettled: boolean,
    shouldReasons: string[],
    answers: Comments[],
    settledAnswersCount: number,
    showSettledAnswers: boolean,
}


Vue.component('comment-section-component',
    {
        props: ['questionId'],
        data() {
            return {
                isLoggedIn: IsLoggedIn.Yes,
                comments: [] as Comments[],
                settledComments: [] as Comments[],
                currentUserImageUrl: '/Images/Users/5933_128s.jpg?t=20210811103604',
                showSettledComments: false
            };
        },
        template: '#comment-section-component',
        mounted() {
            this.getComments();
            eventBus.$on('show-comment-section-modal', (id) => {
                this.questionId = id;
                this.getComments();
            });
        },
        methods: {
            getComments() {
                var self = this;
                console.log(self.questionId);
                //$.ajax({
                //    url: '/AnswerComments/GetComments/',
                //    data: {
                //        questionId: self.questionId
                //    },
                //    success(result) {
                //        self.currentUserImageUrl = result.currentUserImageUrl;
                //        self.comments = result.comments;
                //    }
                //});
                var params = {
                    questionId: self.questionId,
                };
                $.post("/AnswerComments/GetComments", params, function (data) {
                    console.log(data);
                    self.comments = data.commentsList;
                    self.currentUserImageUrl = data.currentUserImageUrl;
                    eventBus.$emit('comment-is-loaded');
                });
            }
        }
    });