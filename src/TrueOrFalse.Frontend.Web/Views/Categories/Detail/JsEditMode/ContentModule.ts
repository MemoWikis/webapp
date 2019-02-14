Vue.component('content-module', {

    props: ['origMarkdown'],

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            showMarkdownInfo: false,
            markdown: this.origMarkdown,
        }
    },

    mounted() {
        eventBus.$on("set-edit-mode", state => this.canBeEdited = state);
        eventBus.$on("set-new-markdown", string => this.markdown = string);
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