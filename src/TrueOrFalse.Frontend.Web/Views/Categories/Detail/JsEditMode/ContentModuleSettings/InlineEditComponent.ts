var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions
} = tiptapBuild;


//class Iframe extends tiptap.Node {

//    get name() {
//        return 'iframe'
//    }

//    get schema() {
//        return {
//            attrs: {
//                src: {
//                    default: null,
//                },
//            },
//            group: 'block',
//            selectable: false,
//            parseDOM: [{
//                tag: 'iframe',
//                getAttrs: dom => ({
//                    src: dom.getAttribute('src'),
//                }),
//            }],
//            toDOM: node => ['iframe', {
//                src: node.attrs.src,
//                frameborder: 0,
//                allowfullscreen: 'true',
//            }],
//        }
//    }

//    get view() {
//        return {
//            props: ['node', 'updateAttrs', 'view'],
//            computed: {
//                src: {
//                    get() {
//                        return this.node.attrs.src
//                    },
//                    set(src) {
//                        this.updateAttrs({
//                            src,
//                        })
//                    },
//                },
//            },
//            template: `
//        <div class="iframe">
//          <iframe class="iframe__embed" :src="src"></iframe>
//          <input class="iframe__input" @paste.stop type="text" v-model="src" v-if="view.editable" />
//        </div>
//      `,
//        }
//    }

//}

Vue.component('editor-menu-bar', tiptap.EditorMenuBar);
Vue.component('editor-content', tiptap.EditorContent);
Vue.component('editor-floating-menu', tiptap.EditorFloatingMenu);

Vue.component('text-component',
    {
        props: ['content'],
        data() {
            return {
                //tiptap: Object.keys(tiptap),
                //tiptapUtils: Object.keys(tiptapUtils)No
                //tiptapCommands: Object.keys(tiptapCommands),
                //tiptapExtensions: Object.keys(tiptapExtensions),
                json: null,
                html: null,
                editMode: false,
                editor: new tiptap.Editor({
                    editable: this.editMode,
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
                        new tiptapExtensions.History()
                    ],
                    content: this.content,
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.json = getJSON();
                        this.html = getHTML();
                    },
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

