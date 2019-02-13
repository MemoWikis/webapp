Vue.component('content-module', {

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            showMarkdownInfo: false,
        }
    },

    mounted() {
        eventBus.$on("set-edit-mode", state => this.canBeEdited = state);
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

        editContentModule(markdown) {
            if (this.hoverState) {
                console.log(markdown);
            }
            
        },
    }
});