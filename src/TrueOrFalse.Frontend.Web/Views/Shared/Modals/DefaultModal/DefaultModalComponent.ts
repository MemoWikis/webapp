declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var defaultModalComponent = Vue.component('default-modal-component',
    {
        template: '#default-modal-component',
        props: ['showCloseButton'],
        methods: {
            closeModal() {
                eventBus.$emit('closeModal');
            }
        }
    })