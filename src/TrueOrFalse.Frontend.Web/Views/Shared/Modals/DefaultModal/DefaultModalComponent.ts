declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var defaultModalComponent = Vue.component('default-modal-component', {
    props: ['showCloseButton'],
    data() {
        return {
            showModal: false,
        }
    },

    created() {
        eventBus.$on('openModal', () => {
            console.log("ShowingModal");
            this.showModal = true;
        });
    },

    methods: {
        closeModal() {
            this.showModal = false;
        }
    }
})