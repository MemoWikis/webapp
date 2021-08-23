declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var defaultModalComponent = Vue.component('default-modal-component',
    {
        props: ['showCloseButton'],
        data() {

        },

        created() {

        },

        methods: {

        }
    })