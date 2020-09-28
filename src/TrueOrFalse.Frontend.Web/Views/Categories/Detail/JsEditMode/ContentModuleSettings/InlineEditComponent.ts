var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions
} = tiptapBuild;

Vue.component('editor-menu-bar', tiptap.EditorMenuBar);
Vue.component('editor-content', tiptap.EditorContent);

Vue.component('text-component',
    {
        props: ['content'],
        data() {
            return {
                tiptap: Object.keys(tiptap),
                tiptapUtils: Object.keys(tiptapUtils),
                tiptapCommands: Object.keys(tiptapCommands),
                tiptapExtensions: Object.keys(tiptapExtensions),
                editor: new tiptap.Editor({
                    content: this.content,
                }),
                htmlContent: "",
            }
        },
        mounted() {
        }
    });

//Vue.component('inline-text-component', {
//    props: {
//        markdown: String,
//        id: String,
//    },

//    template: '#inlinetext-edit',

//    data() {
//        return {
//            parentId: this.$parent.id,
//            textContent: this.$parent.markdown,
//            textAreaId: this.$parent.textAreaId,
//            textCanBeEdited: this.$parent.textCanBeEdited,
//            textBeforeEdit: '',

//        };
//    },

//    watch: {
//        textContent: function() {
//            this.$parent.markdown = this.textContent;
//        },
//    },

//    mounted: function () {
//        this.$refs[this.textAreaId].$el.focus();
//        this.textBeforeEdit = this.textContent;
//    },

//    methods: {

//        applyNewMarkdown() {
//            if (this.textCanBeEdited) {
//                this.$parent.isListening = true;
//                Utils.ApplyMarkdown(this.textContent, this.parentId);
//            }
//        },

//        cancelTextEdit() {
//            this.textContent = this.textBeforeEdit;
//            this.$parent.markdown = this.textContent;
//            this.$parent.textCanBeEdited = false;
//            this.$parent.hoverState = false;
//        }
//    },
//});

