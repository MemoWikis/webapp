Vue.component('answer-comment-component',
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
            }
        },

        mounted() {

        },

        methods: {
        }
    });