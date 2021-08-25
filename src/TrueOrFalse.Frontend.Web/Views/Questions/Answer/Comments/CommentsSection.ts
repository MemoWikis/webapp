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
        data() {
            return {
                isLoggedIn: IsLoggedIn.Yes,
                comments: [] as Comments[],
                settledComments: [] as Comments[],
                questionId: 0,
                currentUserImageUrl: ''
            };
        },
        template: '#comment-section-component',
        mounted() {
            eventBus.$on('show-comment-section-modal', (id) => {
                this.getComments();
                this.questionId = id;
            });
        },
        methods: {
            getComments() {
                var self = this;
                $.ajax({
                    url: '/AnswerComments/GetComments/',
                    data: {
                        questionId: self.questionId
                    },
                    success(result) {
                        self.currentUserImageUrl = result.currentUserImageUrl;
                        self.comments = result.comments;
                    }
                });
            }
        }
    });