declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var errorModalComponent = Vue.component('error-modal-component',
    {
        data() {
            return {
                message: '',
            }
        },
        mounted() {
            eventBus.$on('show-error',
                (msg) => {
                    this.message = msg;
                    $('#ErrorModal').modal('show');
                });
            $('#ErrorModal').on('show.bs.modal',
                event => {
                });

            $('#ErrorModal').on('hidden.bs.modal',
                () => {
                    this.message = '';
                });
        }
    });

