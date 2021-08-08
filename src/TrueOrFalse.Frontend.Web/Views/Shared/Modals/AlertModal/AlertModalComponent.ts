declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();
enum MessageType
{
    Category = 1,
    Question = 2,
    User = 3,
}
var messages = {
    "success": {
        "category": [
            "Dein Thema wurde erfolgreich veröffentlicht.",
            "Das Thema wurde erfolgreich auf 'Privat' gesetzt.",
            "Die Verknüpfung wurde erfolgreich gelöst."
            ],
        "question": [
            "Deine Frage wurde erfolgreich gespeichert.",
            ],
        "user": [],
    },
    "error": {
        "category": [
            "Veröffentlichung ist nicht möglich. Das übergeordnete Thema ist privat.",
            "Dieses Thema hat öffentliche untergeordnete Themen.",
            "Dieses Thema hat öffentliche untergeordnete Fragen.",
            "Dieses Thema kann nicht gelöscht werden, da weitere Themen untergeordnet sind. Bitte entferne alle Unterthemen und versuche es erneut.",
            "Die Verknüpfung des Themas kann nicht gelöst werden. Das Thema muss mindestens einem Oberthema zugeordnet sein.",
            ],
        "question": [
            "Der Fragetext fehlt.",
            "Deine Frage konnte nicht gespeichert werden."
        ],
        "user": [],
    }
}


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
                    switch (data.type) {
                    case MessageType.Category:
                        this.message = messages.success.category[data.id];
                        break;                  
                    case MessageType.Question:  
                        this.message = messages.success.question[data.id];
                        break;                  
                    case MessageType.User:      
                        this.message = messages.success.user[data.id];
                        break;
                    }
                    if (data.reload)
                        this.reload = data.reload;
                    $('#SuccessModal').modal('show');

                });

            $('#SuccessModal').on('hidden.bs.modal',
                () => this.clearData());

            eventBus.$on('show-error',
                (data) => {
                    switch (data.type) {
                        case MessageType.Category:
                            this.message = messages.error.category[data.id];
                            break;
                        case MessageType.Question:
                            this.message = messages.error.question[data.id];
                            break;
                        case MessageType.User:
                            this.message = messages.error.user[data.id];
                            break;
                    }
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

