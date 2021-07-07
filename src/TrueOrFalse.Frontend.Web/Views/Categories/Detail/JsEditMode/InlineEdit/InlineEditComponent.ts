var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions,
} = tiptapBuild;

var {
    tiptapVue,
    tiptapStarterKit,
} = tiptapBuildV2;

var {
    apache,
    //cLike,
    xml,
    bash,
    //c,
    coffeescript,
    csharp,
    css,
    markdown,
    diff,
    ruby,
    go,
    http,
    ini,
    java,
    javascript,
    json,
    kotlin,
    less,
    lua,
    makefile,
    perl,
    nginx,
    objectivec,
    php,
    phpTemplate,
    plaintext,
    properties,
    python,
    pythonREPL,
    rust,
    scss,
    shell,
    sql,
    swift,
    yaml,
    typescript,
} = hljsBuild;
Vue.component('editor-menu-bar', tiptap.EditorMenuBar);
Vue.component('editor-menu-bubble', tiptap.EditorMenuBubble);
Vue.component('editor-content', tiptap.EditorContent);
Vue.component('editor-floating-menu', tiptap.EditorFloatingMenu);

Vue.component('text-component',
    {
        props: ['content'],
        data() {
            return {
                linkUrl: null,
                linkMenuIsActive: false,
                json: null,
                html: this.content,
                contentHasBeenSaved: false,
                contentIsChanged: false,
                savedContent: null,
                editor: new tiptap.Editor({
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
                        new tiptapExtensions.Link({
                            HTMLAttributes: { target: '_self', rel: 'noopener noreferrer nofollow' }
                        }),
                        new tiptapExtensions.Image(),
                        new tiptapExtensions.Bold(),
                        //new tiptapExtensions.Code(),
                        new tiptapExtensions.Italic(),
                        new tiptapExtensions.Strike(),
                        new tiptapExtensions.Underline(),
                        new tiptapExtensions.History(),
                        new tiptapExtensions.TrailingNode({
                            node: 'paragraph',
                            notAfter: ['paragraph'],
                        }),
                        new tiptapExtensions.CodeBlockHighlight({
                            languages: {
                                apache,
                                //cLike,
                                xml,
                                bash,
                                //c,
                                coffeescript,
                                csharp,
                                css,
                                markdown,
                                diff,
                                ruby,
                                go,
                                http,
                                ini,
                                java,
                                javascript,
                                json,
                                kotlin,
                                less,
                                lua,
                                makefile,
                                perl,
                                nginx,
                                objectivec,
                                php,
                                phpTemplate,
                                plaintext,
                                properties,
                                python,
                                pythonREPL,
                                rust,
                                scss,
                                shell,
                                sql,
                                swift,
                                yaml,
                                typescript,
                            },
                        }),
                        new tiptapExtensions.Placeholder({
                            emptyEditorClass: 'is-editor-empty',
                            emptyNodeClass: 'is-empty',
                            emptyNodeText: 'Klicke hier um zu tippen ...',
                            showOnlyWhenEditable: true,
                            showOnlyCurrent: true,
                        }),
                    ],
                    content: this.content,
                    editorProps: {
                        handleKeyDown: (e, k) => {
                            this.contentIsChanged = true;
                        },
                    },
                    onPaste: () => {
                        this.contentIsChanged = true;
                    },
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.json = getJSON();
                        this.html = getHTML();
                    },
                    nativeExtensions: [
                        ]
                }),
            }
        },
        created() {
            this.$root.content = this.html;
        },
        mounted() {
            eventBus.$on('save-success',
                () => {
                    this.contentHasBeenSaved = true;
                    this.savedContent = this.html;
                });
            eventBus.$on('cancel-edit-mode',
                () => {
                    var newContent;
                    if (this.contentHasBeenSaved)
                        newContent = this.savedContent;
                    else
                        newContent = this.content;
                    this.contentIsChanged = false;
                    this.editor.destroy();
                    this.editor =
                        new tiptap.Editor({
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
                                new tiptapExtensions.Link({
                                    target: "_self"
                                }),
                                new tiptapExtensions.Image(),
                                new tiptapExtensions.Bold(),
                                //new tiptapExtensions.Code(),
                                new tiptapExtensions.Italic(),
                                new tiptapExtensions.Strike(),
                                new tiptapExtensions.Underline(),
                                new tiptapExtensions.History(),
                                new tiptapExtensions.TrailingNode({
                                    node: 'paragraph',
                                    notAfter: ['paragraph'],
                                }),
                                new tiptapExtensions.CodeBlockHighlight({
                                    languages: {
                                        apache,
                                        //cLike,
                                        xml,
                                        bash,
                                        //c,
                                        coffeescript,
                                        csharp,
                                        css,
                                        markdown,
                                        diff,
                                        ruby,
                                        go,
                                        http,
                                        ini,
                                        java,
                                        javascript,
                                        json,
                                        kotlin,
                                        less,
                                        lua,
                                        makefile,
                                        perl,
                                        nginx,
                                        objectivec,
                                        php,
                                        phpTemplate,
                                        plaintext,
                                        properties,
                                        python,
                                        pythonREPL,
                                        rust,
                                        scss,
                                        shell,
                                        sql,
                                        swift,
                                        yaml,
                                        typescript,
                                    },
                                }),
                                new tiptapExtensions.Placeholder({
                                    emptyEditorClass: 'is-editor-empty',
                                    emptyNodeClass: 'is-empty',
                                    emptyNodeText: 'Klicke hier um zu tippen ...',
                                    showOnlyWhenEditable: true,
                                    showOnlyCurrent: true,
                                })
                            ],
                            content: newContent,
                            editorProps: {
                                handleKeyDown: () => {
                                    this.contentIsChanged = true;
                                },
                            },
                            onPaste: () => {
                                this.contentIsChanged = true;
                            },
                            onUpdate: ({ getJSON, getHTML }) => {
                                this.json = getJSON();
                                this.html = getHTML();
                            },
                            nativeExtensions: [
                            ],
                        });
                });
        },
        watch: {
            html() {
                this.$root.content = this.html;
                if (this.contentIsChanged)
                    eventBus.$emit('content-change');
            },

            json() {
                this.$root.json = this.json;
            }
        },
        methods: {
            showLinkMenu(attrs) {
                this.linkUrl = attrs.href;
                this.$nextTick(() => {
                    this.$refs.linkInput.focus();
                });
            },
            hideLinkMenu() {
                this.linkUrl = null;
                $('#inlineEditLinkModal').modal('hide');
            },
            setLinkUrl(command, url) {
                command({ href: url });
                this.hideLinkMenu();
            },
        },
    });

