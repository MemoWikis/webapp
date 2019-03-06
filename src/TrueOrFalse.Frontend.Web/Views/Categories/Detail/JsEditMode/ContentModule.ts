var contentModuleComponent = Vue.component('content-module', {

    props: {
        origMarkdown: String,
        contentModuleType: String,
    },

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            markdown: '',
            isListening: false,
            modalType: '',
            id: '',
            modal: '',
            button: '',
            textCanBeEdited: false,
            dataTarget: '',
        };
    },

    created() {
        if (this.contentModuleType != "inlinetext") {
            this.modalType = '#' + this.contentModuleType + 'SettingsDialog';
        }
        this.id = this.contentModuleType + 'Module-' + (this._uid - 2);
    },

    mounted() {
        eventBus.$on('set-edit-mode', state => this.canBeEdited = state);
        eventBus.$on('close-content-module-settings-modal', (event) => {
            if (this.isListening && event) {
                this.isDeleted = true;
                this.isListening = false;
            } else if (this.isListening && !event) {
                this.isListening = false;
            };
        });
    },

    watch: {
        canBeEdited: function (val) {
            if (val) {
                if (this.contentModuleType != 'inlinetext') {
                    this.modal = 'modal';
                    this.button = 'button';
                }
                this.dataTarget = this.modalType;
                this.markdown = this.origMarkdown;
            } else {
                this.modal = '';
                this.button = '';
                this.markdown = '';
                this.dataTarget = '';
            };
        },
    },

    methods: {

        updateHoverState(isHover) {
            const self = this;
            if (self.canBeEdited) {
                self.hoverState = isHover;
            }
        },

        deleteModule() {
            const self = this;
            self.isDeleted = true;
        },

        editModule() {
            if (this.contentModuleType != 'inlineText') {
                this.isListening = true;
                let parent = {
                    id: this.id,
                    markdown: this.markdown,
                };
                $(this.modalType).data('parent', parent).modal('show');
            };
        },

        editInlineText() {
            if (this.canBeEdited) {
                this.textCanBeEdited = true;
                this.isListening = true;
            };
        },

        moveUp() {

        },

        moveDown() {

        },
    },
});