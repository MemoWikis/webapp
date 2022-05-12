declare var editorContent: any;

Vue.component('text-component',
    {
        props: ['content'],
        template: '#text-component',
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
                menuBarComponentKey: '0',
                indexTimer: null,
                editable: true,
                images: [],
            }
        },
        created() {
            this.$root.content = this.html;
        },
        mounted() {
            editorContent = Vue.component('editor-content', tiptapEditorContent);
            this.editor = new tiptapEditor({
                editable: this.editable,
                content: this.content,
                extensions: [
                    tiptapStarterKit.configure({
                        heading: {
                            levels: [2, 3],
                            HTMLAttributes: {
                                class: 'inline-text-heading'
                            }
                        }
                    }),
                    tiptapLink.configure({
                        HTMLAttributes: {
                            rel: 'noopener noreferrer nofollow'
                        },
                        openOnClick: true,
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
                    }),
                    tiptapUnderline,
                    tiptapImage.configure({
                        inline: true,
                        allowBase64: true,
                    })
                ],
                editorProps: {
                    handleKeyDown: (e, k) => {
                        this.contentIsChanged = true;
                    },
                    handleClick: (view, pos, event) => {
                        var _a;
                        var attrs = this.editor.getAttributes('link');
                        var href = Site.IsMobile ? event.target.href : attrs.href;
                        var link = (_a = event.target) === null || _a === void 0 ? void 0 : _a.closest('a');
                        if (link && href) {
                            window.open(href, event.ctrlKey ? '_blank' : '_self');
                            return true;
                        }
                        return false;
                    },
                    handlePaste: (view, pos, event) => {
                        let eventContent = event.content.content;
                        var v = this;
                        if (eventContent.length >= 1 && !_.isEmpty(eventContent[0].attrs)) {
                            let src = eventContent[0].attrs.src;
                            if (src.length > 1048576 && src.startsWith('data:image')) {
                                Alerts.showError({
                                    text: messages.error.image.tooBig
                                });
                                return true;
                            } else if (src.startsWith('data:image')) {
                            }
                        }
                    },
                    attributes: {
                        id: 'InlineEdit',
                    }
                },
                onPaste: () => {
                    this.contentIsChanged = true;
                },
                onUpdate: ({ editor }) => {
                    this.json = editor.getJSON();
                    this.html = editor.getHTML();
                    this.handleImage(editor);
                },
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
                            editable: this.editable,
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
                                }),
                                tiptapImage.configure({
                                    inline: true,
                                    allowBase64: true,
                                })
                            ],
                            editorProps: {
                                handleKeyDown: (e, k) => {
                                    this.contentIsChanged = true;
                                },
                                handlePaste: (view, pos, event) => {
                                    let eventContent = event.content.content;
                                    if (eventContent.length >= 1 && !_.isEmpty(eventContent[0].attrs)) {
                                        let src = eventContent[0].attrs.src;
                                        if (src.length > 1048576 && src.startsWith('data:image')) {
                                            Alerts.showError({
                                                text: messages.error.image.tooBig
                                            });
                                            return true;
                                        }
                                    }
                                },
                            },
                            onPaste: () => {
                                this.contentIsChanged = true;
                            },
                            onUpdate: ({editor}) => {
                                this.json = this.editor.getJSON();
                                this.html = this.editor.getHTML();
                                this.handleImage(editor);
                            },
                        });
                    this.menuBarComponentKey = !this.menuBarComponentKey;
                });

        },
        watch: {
            html(val) {
                this.$root.content = this.html;
                if (this.contentIsChanged)
                    eventBus.$emit('content-change');
            },

            json() {
                this.$root.json = this.json;
            },
            editable() {
                this.editor.setEditable(this.editable);
            },
        },
        methods: {
            handleImage(editor) {
                let images = [];
                editor.state.doc.descendants((node, pos) => {
                    if (node.type.name === 'image') {
                        images.push(node.attrs.src);
                    }
                });

                this.images = images;
            },
        },
    });