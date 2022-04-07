declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();
const messages = {
    success: {
        category: {
            publish: "Dein Thema wurde erfolgreich veröffentlicht.",
            setToPrivate: "Das Thema wurde erfolgreich auf 'Privat' gesetzt.",
            unlinked: "Die Verknüpfung wurde erfolgreich gelöst.",
            addedToPersonalWiki: "Das Thema wurde erfolgreich zu deinem Wiki hinzugefügt."
        },
        question: {
            created: "Deine Frage wurde erfolgreich erstellt.",
            saved: "Deine Frage wurde erfolgreich gespeichert.",
            delete: "Deine Frage wurde erfolgreich gelöscht."
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
            isLinkedInNonWuwi: "Du hast das Thema außerhalb deines Wunschwissens schon untergeordnet, bitte stelle: 'Zeige nur dein Wunschwissen' aus und füge die Kategorie deinem Wunschwissen hinzu. ",
            childIsParent: "Übergeordnete Themen können nicht untergeordnet werden.",
            nameIsTaken: " ist bereits vergeben, bitte wähle einen anderen Namen!",
            nameIsForbidden: " ist verboten, bitte wähle einen anderen Namen!",
            rootCategoryMustBePublic: "Das Root Thema kann nicht auf privat gesetzt werden.",
            missingRights: "Dir fehlen die notwendigen Rechte.",
            tooPopular: "Dieses Thema ist zu oft im Wunschwissen anderer User"
        },
        question: {
            missingText: "Der Fragetext fehlt.",
            missingAnswer: "Die Antwort zur Frage fehlt.",
            save: "Deine Frage konnte nicht gespeichert werden.",
            creation: "Deine Frage konnte nicht erstellt werden.",
            isInWuwi: (count: number | string) =>
                `Die Frage kann nicht gelöscht werden, sie ist ${count}x Teil des Wunschwissens anderer Nutzer. Bitte melde dich bei uns, wenn du meinst, die Frage sollte dennoch gelöscht werden.`,
            rights: "Dir fehlt die Berechtigung dazu.",
            errorOnDelete: "Es ist ein Fehler aufgetreten! Möglicherweise sind Referenzen auf die Frage (Lernsitzungen, Termine, Wunschwissen-Einträge...) teilweise gelöscht."
        },
        user: {
            emailInUse: "Die Email-Adresse ist bereits in Verwendung."
        },
        default: "Ein Fehler ist aufgetreten.",
        image: {
            tooBig: "Das Bild ist zu groß. Die Dateigröße darf maximal 1MB betragen."
        }
    },
    info: {
        category: {},
        question: {
            newQuestionNotInFilter: '<b>Achtung: Die Frage wird dir nach dem Erstellen nicht angezeigt,</b> da die gewählten Optionen nicht mit den Filtereinstellungen übereinstimmen, Passe den lernfilter an, um die Frage anzuzeigen.'
        },
    }
}

type AlertMsg = {
    text: string,
    reload?: boolean,
    customHtml?: string,
    customBtn?: string,
}

class Alerts {
    static showError(msg: AlertMsg): void {
        eventBus.$emit('show-error', msg);
    }
    static showSuccess(msg: AlertMsg): void {
        eventBus.$emit('show-success', msg);
    }
}

Vue.component('alert-modal-component',
    {
        data() {
            return {
                error: true,
                message: '',
                reload: false,
                customHtml: '',
                customBtn: '',
            }
        },
        mounted() {
            eventBus.$on('show-success',
                (data: AlertMsg) => {
                    this.error = false;
                    this.message = data.text;
                    if (data.reload)
                        this.reload = data.reload;
                    if (data.customHtml != null && data.customHtml.length > 0)
                        this.customHtml = data.customHtml;

                    if (data.customBtn != null && data.customBtn.length > 0)
                        this.customBtn = data.customBtn;
                    
                    $('#SuccessModal').modal('show');

                });

            $('#SuccessModal').on('hidden.bs.modal', () => this.clearData());

            eventBus.$on('show-error',
                (data: AlertMsg) => {
                    if (data != null) {
                        this.message = data.text;
                        if (data.reload)
                            this.reload = data.reload;
                    } else
                        this.message = messages.error.default;

                    $('#ErrorModal').modal('show');
                });

            $('#ErrorModal').on('hidden.bs.modal', () => this.clearData());
        },

        methods: {
            clearData() {
                if (this.reload)
                    location.reload();
                this.message = '';
                this.error = true;
                this.reload = false;
                this.customHtml = '';
                this.customBtn = '';
            }
        }
    });

