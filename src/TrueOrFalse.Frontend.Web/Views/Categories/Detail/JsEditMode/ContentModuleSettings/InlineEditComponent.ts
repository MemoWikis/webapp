var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions
} = tiptapBuild;

var hljs: any;

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
                        new tiptapExtensions.CodeBlockHighlight({
                            languages: {
                            },
                        })
                    ],
                    content: this.content,
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.json = getJSON();
                        this.html = getHTML();
                    },
                    editorProps: {
                        handleDOMEvents: {
                            drop: (view, e) => { e.preventDefault(); },
                        }
                    },
                    // hide the drop position indicator
                    dropCursor: { width: 0, color: 'transparent' },
                }),
            }
        },
        created() {
            this.setContent(this.content);
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
                this.setContent(this.html);
            }
        },
        methods: {
            escapeHtml(unsafe) {
                return unsafe
                    .replace(/&/g, "&amp;")
                    .replace(/</g, "&lt;")
                    .replace(/>/g, "&gt;")
                    .replace(/"/g, "&quot;")
                    .replace(/'/g, "&#039;");
            },
            setContent(html) {
                var escaped = this.escapeHtml(html);
                var json = {
                    "TemplateName": "InlineText",
                    "Content": html
                }
                this.$parent.content = json;
                this.htmlContent = html;

            }
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

