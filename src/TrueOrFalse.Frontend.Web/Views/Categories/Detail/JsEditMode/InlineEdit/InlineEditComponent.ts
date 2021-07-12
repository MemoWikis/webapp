//var {
//    tiptapVue,
//    tiptapStarterKit,
//    tiptapExtensionCodeBlockLowlight,
//    tiptapExtensionImage,
//    tiptapExtensionLink,
//    tiptapExtensionDocument,
//    tiptapExtensionText,
//    tiptapExtensionParagraph
//} = tiptapBuild;

//var {
//    apache,
//    //cLike,
//    xml,
//    bash,
//    //c,
//    coffeescript,
//    csharp,
//    css,
//    markdown,
//    diff,
//    ruby,
//    go,
//    http,
//    ini,
//    java,
//    javascript,
//    json,
//    kotlin,
//    less,
//    lua,
//    makefile,
//    perl,
//    nginx,
//    objectivec,
//    php,
//    phpTemplate,
//    plaintext,
//    properties,
//    python,
//    pythonREPL,
//    rust,
//    scss,
//    shell,
//    sql,
//    swift,
//    yaml,
//    typescript,
//} = hljsBuild;

declare var tiptapEditor: any;
declare var tiptapEditorContent: any;
declare var tiptapStarterKit: any;
declare var tiptapLink: any;
declare var tiptapCodeBlockLowlight: any;
declare var lowlight: any;

Vue.component('editor-content', tiptapEditorContent);

Vue.component('editor-menu-bar-component',
    {
        props: ['editor'],
        template: '#editor-menu-bar-template',
        data() {
            return {
                focused: false,
            }
        },
        mounted() {
            this.editor.on('focus', () => this.focused = true);
            this.editor.on('blur', () => this.focused = false);
        },
        methods: {
            setLink() {
                const url = window.prompt('URL');

                this.editor
                    .chain()
                    .focus()
                    .extendMarkRange('link')
                    .setLink({ href: url })
                    .run();
            },
        }
    });

var textComponent = Vue.component('text-component',
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
                editor: null,
            }
        },
        created() {
            this.$root.content = this.html;
        },
        mounted() {
            this.editor = new tiptapEditor({
                content: this.content,
                extensions: [
                    tiptapStarterKit.configure({
                        heading: {
                            levels: [2, 3]
                        }
                    }),
                    tiptapLink.configure({
                        HTMLAttributes: {
                            target: '_self',
                            rel: 'noopener noreferrer nofollow'
                        }
                    }),
                    tiptapPlaceholder.configure({
                        emptyEditorClass: 'is-editor-empty',
                        emptyNodeClass: 'is-empty',
                        placeholder: 'Klicke hier um zu tippen ...',
                        showOnlyWhenEditable: true,
                        showOnlyCurrent: true,
                    }),
                    tiptapCodeBlockLowlight.configure({
                        lowlight,
                    })
                ],
                editorProps: {
                    handleKeyDown: (e, k) => {
                        this.contentIsChanged = true;
                    },
                },
                onPaste: () => {
                    this.contentIsChanged = true;
                },
                onUpdate: ({ editor }) => {
                    this.json = editor.getJSON();
                    this.html = editor.getHTML();
                },
                nativeExtensions: [
                ]
            });
            eventBus.$on('save-success',
                () => {
                    this.contentHasBeenSaved = true;
                    this.savedContent = this.html;
                });
            eventBus.$on('cancel-edit-mode',
                () => {
                    var newContent = this.contentHasBeenSaved ? this.savedContent : this.content;
                    this.contentIsChanged = false;
                    this.editor.destroy();
                    this.editor =
                        new tiptapEditor({
                            content: newContent,
                            extensions: [
                                tiptapStarterKit.configure({
                                    heading: {
                                        levels: [2, 3]
                                    }
                                }),
                                tiptapLink.configure({
                                    HTMLAttributes: {
                                        target: '_self',
                                        rel: 'noopener noreferrer nofollow'
                                    }
                                }),
                                tiptapPlaceholder.configure({
                                    emptyEditorClass: 'is-editor-empty',
                                    emptyNodeClass: 'is-empty',
                                    placeholder: 'Klicke hier um zu tippen ...',
                                    showOnlyWhenEditable: true,
                                    showOnlyCurrent: true,
                                }),
                                tiptapCodeBlockLowlight.configure({
                                    lowlight,
                                })
                            ],
                            editorProps: {
                                handleKeyDown: (e, k) => {
                                    this.contentIsChanged = true;
                                },
                            },
                            onPaste: () => {
                                this.contentIsChanged = true;
                            },
                            onUpdate: () => {
                                this.json = this.editor.getJSON();
                                this.html = this.editor.getHTML();
                            },
                            nativeExtensions: [
                            ]
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