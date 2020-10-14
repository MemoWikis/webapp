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
                        }),
                        new tiptapExtensions.Placeholder({
                            emptyEditorClass: 'is-editor-empty',
                            emptyNodeClass: 'is-empty',
                            emptyNodeText: 'Write something …',
                            showOnlyWhenEditable: true,
                            showOnlyCurrent: true,
                        }),
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

            setContent(html) {
                var json = {
                    "TemplateName": "InlineText",
                    "Content": html
                }
                this.$parent.content = json;
                this.htmlContent = html;

            }
        }
    });

