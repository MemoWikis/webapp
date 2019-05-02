
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
            textAreaId: this.$parent.textAreaId,
            textCanBeEdited: this.$parent.textCanBeEdited,
        };
    },

    mounted: function () {
        this.$refs[this.textAreaId].$el.focus();
    },

    methods: {
        changeContent(val) {
            this.textContent = val;
        },

        applyNewMarkdown() {
            this.$parent.isListening = true;
            Utils.ApplyMarkdown(this.textContent, this.parentId);
        },

        cancelTextEdit() {
            this.$parent.textCanBeEdited = false;
            this.$parent.hoverState = false;
        }
    },
});

