
var FAB = Vue.component('floating-action-button',
    {
        props: ['tab'],
        data() {
            return {
                editMode: false,
                isOpen: false,
            }
        },
        methods: {
            toggleFAB() {
                this.isOpen = !this.isOpen;
                console.log(this.isOpen)
            }
        }
    });

var FABContainer = new Vue({
    el: '#FloatingActionButtonApp',

})