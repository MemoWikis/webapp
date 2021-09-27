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
        created() {
            this.getCurrentUserImgUrl();
            this.getComments(6338);

            eventBus.$on('load-comment-section-modal', (questionId) => {
                //this.getComments(questionId);
            });

        },
        methods: {
            getComments(questionId) {
                const self = this;
                self.questionId = questionId;
                $.post("/AnswerComments/GetComments?questionId=" + self.questionId, data => {
                    self.comments = JSON.parse(data);
                    eventBus.$emit("comment-is-loaded");
                });
            },
            getCurrentUserImgUrl() {
                const self = this;
                $.post("/AnswerComments/GetCurrentUserImgUrl", url => {
                    self.currentUserImageUrl = url as String;
                });
            }
        }
    });