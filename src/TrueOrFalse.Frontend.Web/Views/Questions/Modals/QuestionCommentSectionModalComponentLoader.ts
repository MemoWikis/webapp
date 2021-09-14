Vue.component('question-comment-section-modal-component',
    {
        template: '#question-comment-section-modal-component',
        props: ['commentIsLoaded', 'questionId'],
        data() {
            return {
        }
        },
        beforeCreate() {
            eventBus.$on('closeModal', () => {
                this.commentIsLoaded = false;
            });

        },
    });
