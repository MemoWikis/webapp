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
                ctrlIsHeld: false,
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
                            target: '_self',
                            rel: 'noopener noreferrer nofollow'
                        },
                        openOnClick: !this.ctrlIsHeld,
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
                            this.ctrlIsHeld = true;
                    },
                    handleClickOn: (v, p, n, nP, e, d) => {
                        if (e.target.nodeName == 'A' && this.ctrlIsHeld) {
                            window.open(e.target.href, '_blank');
                            return;
                        }
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
                    //var children = editor.view.docView.children;
                    //var foundHeading = children.find(c => c.node.type.name == 'heading') != undefined;

                    //var self = this;
                    //self.updateIndex();
                    //if (foundHeading) {
                    //    clearTimeout(self.indexTimer);
                    //    this.indexTimer = setTimeout(() => {
                    //        },
                    //        1000);
                    //}

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