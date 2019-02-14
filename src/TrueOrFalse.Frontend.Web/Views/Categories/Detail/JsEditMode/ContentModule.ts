Vue.component('content-module', {

    props: ['origMarkdown'],

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            showMarkdownInfo: false,
            markdown: '',
            componentId: '',
        }
    },

    created() {
        this.markdown = this.origMarkdown;
        this.componentId = this._uid;
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

        getMarkdown(string) {
            console.log(string);
        },
    }
});