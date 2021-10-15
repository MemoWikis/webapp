Vue.component('question-comment-section-modal-component',
    {
        template: '#question-comment-section-modal-component',
        props: ['commentIsLoaded', 'questionId'],
        data() {
            return {
                commentsCount: 0,
        }
        },
        beforeCreate() {
            eventBus.$on('close-modal', () => {
                eventBus.$emit('unload-comment');
            });
            eventBus.$on('send-coomments-count', function (count) {
                this.commentCount = count;
            });

        },
        mounted() {

        }
    });
