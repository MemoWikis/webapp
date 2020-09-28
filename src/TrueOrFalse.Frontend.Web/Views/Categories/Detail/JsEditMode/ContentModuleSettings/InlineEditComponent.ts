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
                //tiptap: Object.keys(tiptap),
                //tiptapUtils: Object.keys(tiptapUtils),
                //tiptapCommands: Object.keys(tiptapCommands),
                //tiptapExtensions: Object.keys(tiptapExtensions),
                editMode: false,
                editor: new tiptap.Editor({
                    editable: this.editMode,
                    extensions: [
                        new tiptapExtensions.History()
                    ],
                    content: this.content,
                }),
                htmlContent: "",
            }
        },
        mounted() {
            eventBus.$on("set-edit-mode",
                (state) => {
                    this.editMode = state;
                });
        },
        watch: {
            editMode() {
                this.editor.setOptions({
                    editable: this.editMode,
                });
            },
        },
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

