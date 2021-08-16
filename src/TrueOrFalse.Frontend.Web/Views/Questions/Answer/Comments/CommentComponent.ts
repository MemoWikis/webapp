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
        }
        },

        mounted() {

        },

        methods: {
        }
    });
Vue.component('comment-answer-add',
    {
        props: [],
        data() {
            return {
            }
        },

        mounted() {

        },

        methods: {
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
