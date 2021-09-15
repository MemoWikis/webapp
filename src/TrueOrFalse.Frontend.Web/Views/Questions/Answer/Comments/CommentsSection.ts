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
                questionId: 0,
                isLoggedIn: IsLoggedIn.Yes,
                comments: [] as Comments[],
                settledComments: [] as Comments[],
                currentUserImageUrl: '',
                showSettledComments: false
            };
        },
        template: '#comment-section-component',
        mounted() {
            eventBus.$on('load-comment-section-modal', (questionId) => {
                this.getComments(questionId);
                this.getCurrentUserImgUrl();
            });

        },
        methods: {
            getComments(questionId) {
                const self = this;
                self.questionId = questionId;
                $.post("/AnswerComments/GetComments?questionId=" + self.questionId, data => {
                    self.comments = data;
                    console.log(data);
                    console.log(this.comments);
                    eventBus.$emit("comment-is-loaded");
                });
            },
            getCurrentUserImgUrl() {
                const self = this;
                $.post("/AnswerComments/GetCurrentUserImgUrl", data => {
                    console.log(data);
                    self.currentUserImageUrl = data;
                });
            }
        }
    });