var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions
} = tiptapBuild;

Vue.component('editor-menu-bar', tiptap.EditorMenuBar);
Vue.component('editor-content', tiptap.EditorContent);
Vue.component('editor-floating-menu', tiptap.EditorFloatingMenu);

Vue.component('text-component',
    {
        props: ['content'],
        data() {
            return {
                json: null,
                html: null,
                htmlContent: null,
                editMode: false,
                editor: new tiptap.Editor({
                    editable: false,
                    extensions: [
                        new tiptapExtensions.Blockquote(),
                        new tiptapExtensions.BulletList(),
                        new tiptapExtensions.CodeBlock(),
                        new tiptapExtensions.HardBreak(),
                        new tiptapExtensions.Heading({ levels: [2, 3] }),
                        new tiptapExtensions.HorizontalRule(),
                        new tiptapExtensions.ListItem(),
                        new tiptapExtensions.OrderedList(),
                        new tiptapExtensions.TodoItem(),
                        new tiptapExtensions.TodoList(),
                        new tiptapExtensions.Link(),
                        new tiptapExtensions.Bold(),
                        new tiptapExtensions.Code(),
                        new tiptapExtensions.Italic(),
                        new tiptapExtensions.Strike(),
                        new tiptapExtensions.Underline(),
                        new tiptapExtensions.History(),
                    ],
                    content: this.content,
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.json = getJSON();
                        this.html = getHTML();
                    },
                }),
            }
        },
        created() {
            this.htmlContent = this.content;
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
            html() {
                this.htmlContent = this.html;
                this.$parent.content = this.html;
            }
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

