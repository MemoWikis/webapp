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
                target: '_blank',
                indexTimer: null,
            //    headings: [],
            }
        },
        created() {
            this.$root.content = this.html;
        },
        mounted() {
            editorContent = Vue.component('editor-content', tiptapEditorContent);
            this.editor = new tiptapEditor({
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
                            target: this.target,
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
                    tiptapUnderline,
                    tiptapImage
                ],
                editorProps: {
                    handleKeyDown: (e, k) => {
                        this.contentIsChanged = true;
                        if (k.ctrlKey == true)
                            this.target = '_blank';
                    },
                    handlePaste: (view, pos, event) => {
                        let eventContent = event.content.content;
                        if (eventContent.length >= 1 && !_.isEmpty(eventContent[0].attrs)) {
                            let src = eventContent[0].attrs.src;
                            if (src.length > 1048576 && src.startsWith('data:image')) {
                                let data = {
                                    msg: messages.error.image.tooBig
                                }
                                eventBus.$emit('show-error', data);
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
                    var children = editor.view.docView.children;
                    var foundHeading = children.find(c => c.node.type.name == 'heading') != undefined;

                    var self = this;
                    self.updateIndex();
                    if (foundHeading) {
                        clearTimeout(self.indexTimer);
                        this.indexTimer = setTimeout(() => {
                            },
                            1000);
                    }

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
                                            let data = {
                                                msg: messages.error.image.tooBig
                                            }
                                            eventBus.$emit('show-error', data);
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
        //    updateIndex() {
        //        var headings = []
        //        var transaction = this.editor.state.tr;

        //        var self = this;

        //        this.editor.state.doc.descendants((node, pos) => {
        //            if (node.type.name == 'heading') {
        //                var id = `heading-${headings.length + 1}`
        //                if (node.attrs.id !== id) {
        //                    transaction.setNodeMarkup(pos, undefined, {
        //                        ...node.attrs,
        //                        id,
        //                    })
        //                }

        //                headings.push({
        //                    level: node.attrs.level,
        //                    text: node.textContent,
        //                    id,
        //                })
        //            }
        //        })
                
        //        transaction.setMeta('preventUpdate', true)
        //        console.log(transaction)
        //        self.editor.view.dispatch(transaction)

        //        this.headings = headings
        //    },
        },
    });