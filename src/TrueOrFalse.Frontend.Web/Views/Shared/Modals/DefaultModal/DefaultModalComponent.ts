declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var defaultModalComponent = Vue.component('default-modal-component',
    {
        template: '#default-modal-component',
        props: ['showCloseButton'],

        created() {
            document.body.classList.add('no-scroll');
        },

        methods: {
            closeModal() {
                document.body.classList.remove('no-scroll');
                eventBus.$emit('close-modal');
            }
        }
    })