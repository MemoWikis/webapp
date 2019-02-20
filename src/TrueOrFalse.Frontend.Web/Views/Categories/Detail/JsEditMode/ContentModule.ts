Vue.component('content-module', {

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
        }
    },

    created() {
        this.markdown = this.origMarkdown;
        this.modalType = '#' + this.contentModuleType + 'ContentModuleSettings';
    },

    mounted() {
        eventBus.$on('set-edit-mode', state => this.canBeEdited = state);
        eventBus.$on('close-content-module-settings-modal', (event) => {
            if (this.isListening && event == true) {
                this.isDeleted = true;
                this.isListening = false;
            } else if (this.isListening && event == false) {
                this.isListening = false;
            }
        });
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

        getMarkdown(string) {
            console.log(string);
        },
    }
});