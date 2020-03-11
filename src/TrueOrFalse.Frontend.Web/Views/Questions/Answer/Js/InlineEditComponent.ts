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
            textBeforeEdit: '',
        };
    },

    watch: {
        textContent: function() {
            this.$parent.markdown = this.textContent;
        },
    },

    mounted: function () {
        this.$refs[this.textAreaId].$el.focus();
        this.textBeforeEdit = this.textContent;
    },

    methods: {

        applyNewMarkdown() {
            if (this.textCanBeEdited) {
                this.$parent.isListening = true;
                Utils.ApplyMarkdown(this.textContent, this.parentId);
            }
        },

        cancelTextEdit() {
            this.textContent = this.textBeforeEdit;
            this.$parent.markdown = this.textContent;
            this.$parent.textCanBeEdited = false;
            this.$parent.hoverState = false;
        }
    },
});

