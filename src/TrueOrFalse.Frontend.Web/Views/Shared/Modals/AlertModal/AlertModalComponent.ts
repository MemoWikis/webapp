declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var alertModalComponent = Vue.component('alert-modal-component',
    {
        data() {
            return {
                error: true,
                message: '',
                reload: false,
            }
        },
        mounted() {
            eventBus.$on('show-success',
                (data) => {
                    this.error = false;
                    this.message = data.msg;
                    if (data.reload)
                        this.reload = data.reload;
                    $('#SuccessModal').modal('show');
                });

            $('#SuccessModal').on('hidden.bs.modal',
                () => this.clearData());

            eventBus.$on('show-error',
                (data) => {
                    this.message = data.msg;
                    if (data.reload)
                        this.reload = data.reload;
                    $('#ErrorModal').modal('show');
                });
            $('#ErrorModal').on('show.bs.modal',
                event => {
                });

            $('#ErrorModal').on('hidden.bs.modal',
                () => this.clearData());
        },

        methods: {
            clearData() {
                if (this.reload)
                    location.reload();
                this.message = '';
                this.error = true;
                this.reload = false;

            }
        }
    });

