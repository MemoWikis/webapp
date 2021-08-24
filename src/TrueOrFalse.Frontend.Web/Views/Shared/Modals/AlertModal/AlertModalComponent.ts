declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();
var messages = {
    success: {
        category: {
            publish: "Dein Thema wurde erfolgreich veröffentlicht.",
            setToPrivate: "Das Thema wurde erfolgreich auf 'Privat' gesetzt.",
            unlinked: "Die Verknüpfung wurde erfolgreich gelöst."
        },
        question: {
            created: "Deine Frage wurde erfolgreich erstellt.",
            saved: "Deine Frage wurde erfolgreich gespeichert.",
        },
        user: {}
    },
    error: {
        category: {
            parentIsPrivate: "Veröffentlichung ist nicht möglich. Das übergeordnete Thema ist privat.",
            publicChildCategories: "Dieses Thema hat öffentliche untergeordnete Themen.",
            publicQuestions: "Dieses Thema hat öffentliche Fragen.",
            notLastChild: "Dieses Thema kann nicht gelöscht werden, da weitere Themen untergeordnet sind. Bitte entferne alle Unterthemen und versuche es erneut.",
            noRemainingParents: "Die Verknüpfung des Themas kann nicht gelöst werden. Das Thema muss mindestens einem Oberthema zugeordnet sein.",
            parentIsRoot: "Unter 'Alle Themem', darfst du nur private Themen neu hinzufügen",
            loopLink: "Man kann keine Themen sich selber unterordnen",
            isAlreadyLinkedAsChild: "Das Thema ist schon untergeordnet.",
            childIsParent: "Übergeordnete Themen können nicht untergeordnet werden.",
            nameIsTaken: " ist bereits vergeben, bitte wähle einen anderen Namen!",
            nameIsForbidden: " ist verboten, bitte wähle einen anderen Namen!",
            rootCategoryMustBePublic: "Das Root Thema kann nicht auf privat gesetzt werden."
        },
        question: {
            missingText: "Der Fragetext fehlt.",
            missingAnswer: "Die Antwort zur Frage fehlt.",
            save: "Deine Frage konnte nicht gespeichert werden.",
            creation: "Deine Frage konnte nicht erstellt werden.",
        },
        user: {}
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

