Vue.component('question-comment-section-modal-component',
    {
        template: '#question-comment-section-modal-component',
        props: ['commentIsLoaded'],
        data() {
            return {
                questionId: 0
        }
        },
        beforeCreate() {
            const self = this;
            eventBus.$on('closeModal', function () {
                self.commentIsLoaded = false;
            });

        },
    });
