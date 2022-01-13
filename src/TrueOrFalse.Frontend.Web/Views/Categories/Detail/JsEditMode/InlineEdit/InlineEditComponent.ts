declare var editorContent: any;
declare var editorTest: any;

//class Guid {
//    static newShortGuid() {
//        return 'xxx-xyx-xxxx'.replace(/[xy]/g, function (c) {
//            var r = Math.random() * 16 | 0,
//                v = c == 'x' ? r : (r & 0x3 | 0x8);
//            return v.toString(16);
//        });
//    }
//}

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
            //    headings: [],
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
                                class: 'heading'
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
                    tiptapImage
                ],
                editorProps: {
                    handleKeyDown: (e, k) => {
                        this.contentIsChanged = true;
                    },
                    handleClick: (view, pos, event) => {
                        var _a;
                        const attrs = this.editor.getAttributes('link');
                        const link = (_a = event.target) === null || _a === void 0 ? void 0 : _a.closest('a');
                        if (link && attrs.href) {
                            window.open(attrs.href, event.ctrlKey ? '_blank' : '_self');
                            return true;
                        }
                        return false;
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

                },
                onFocus({ editor, event }) {
                },
                onBlur({ editor, event }) {
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
                            onUpdate: () => {
                                this.json = this.editor.getJSON();
                                this.html = this.editor.getHTML();
                            },
                            nativeExtensions: [
                            ]
                        });
                    this.menuBarComponentKey = !this.menuBarComponentKey;
                });
            window.editorTest = this.editor;

        },
        watch: {
            html() {
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