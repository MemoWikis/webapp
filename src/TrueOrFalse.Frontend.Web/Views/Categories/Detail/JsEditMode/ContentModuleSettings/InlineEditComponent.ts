
Vue.component('inline-text-component', {
    props: {
        markdown: String,
        id: String,
    },

    template: '#inlinetext-edit',

    data() {
        return {
            parentId: this.$parent.id,
            textContent: this.$parent.markdown,
        };
    },

    created() {
    },

    mounted: function () {
    },

    methods: {
        changeContent(val) {
            this.textContent = val;
        },

        applyNewMarkdown() {
            this.$parent.isListening = true;
            Utils.UpdateMarkdown(this.textContent, this.parentId);
        },

        cancelTextEdit() {
            this.$parent.textCanBeEdited = false;
            this.$parent.hoverState = false;
        }
    },
});

