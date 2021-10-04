interface IComments {
    id: number,
    creatorName: string,
    creationDate: string,
    creationDateNiceText: string,
    imageUrl: string,
    creatorUrl: string,
    text: string,
    shouldBeImproved: boolean,
    shouldBeDeleted: boolean,
    isSettled: boolean,
    shouldReasons: string[],
    answers: IComments[],
    settledAnswersCount: number,
    showSettledAnswers: boolean,
}


Vue.component('comment-section-component',
    {
        props: ['questionId'],

        data() {
            return {
                isLoggedIn: IsLoggedIn.Yes,
                comments: [] as IComments[],
                settledComments: [] as IComments[],
                currentUserImageUrl: '',
                currentUserId: 0,
                showSettledComments: false
            };
        },
        template: '#comment-section-component',
        created() {
            this.getCurrentUserImgUrl();
            this.getCurrentUserId();
            this.getComments(this.questionId);


        },
        methods: {
            getComments(questionId) {
                const self = this;
                self.questionId = questionId;
                $.post("/AnswerComments/GetComments?questionId=" + self.questionId, data => {
                    self.comments = JSON.parse(data) as IComments[];
                    eventBus.$emit("comment-is-loaded");
                });
            },
            getCurrentUserImgUrl() {
                const self = this;
                $.post("/AnswerComments/GetCurrentUserImgUrl", url => {
                    self.currentUserImageUrl = url as String;
                });
            },
            getCurrentUserId() {
                $.post("/AnswerComments/GetCurrentUserId", id => {
                    this.currentUserId = parseInt(id);
                });
            }
        }
    });