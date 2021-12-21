declare var tiptapEditor: any;
declare var tiptapEditorContent: any;
declare var tiptapStarterKit: any;
declare var tiptapLink: any;
declare var tiptapCodeBlockLowlight: any;
declare var tiptapPlaceholder: any;
declare var tiptapUnderline: any;
declare var tiptapImage: any;
declare var lowlight: any;
declare var toHtml: any;

Vue.component('editor-menu-bar-component',
    {
        props: ['editor','heading'],
        template: '#editor-menu-bar-template',
        data() {
            return {
                focused: false,
                clicked: false,
            }
        },
        created() {
            var self = this;
            this.editor.on('focus', () => {
                self.focused = true;
            });
            this.editor.on('blur', () => {
                if (self.clicked) {
                    self.editor.view.dom.focus();
                    self.clicked = false;
                }
                else
                    self.focused = false;
            });
        },
        methods: {
            command(fn) {
                var self = this;
                switch (fn) {
                    case 'bold':
                        self.editor.chain().toggleBold().focus().run();
                        break;
                    case 'italic':
                        self.editor.chain().toggleItalic().focus().run();
                        break;
                    case 'strike':
                        self.editor.chain().toggleStrike().focus().run();
                        break;
                    case 'underline':
                        self.editor.chain().toggleUnderline().focus().run();
                        break;
                    case 'h2':
                        self.editor.chain().toggleHeading({ level: 2 }).focus().run();
                        break;
                    case 'h3':
                        self.editor.chain().toggleHeading({ level: 3 }).focus().run();
                        break;
                    case 'bulletList':
                        self.editor.chain().toggleBulletList().focus().run();
                        break;
                    case 'orderedList':
                        self.editor.chain().toggleOrderedList().focus().run();
                        break;
                    case 'blockquote':
                        self.editor.chain().toggleBlockquote().focus().run();
                        break;
                    case 'codeBlock':
                        self.editor.chain().toggleCodeBlock().focus().run();
                        break;
                    case 'setLink':
                        const linkUrl = window.prompt('Link URL');
                        self.editor.chain().focus().extendMarkRange('link').setLink({ href: linkUrl }).run();
                        break;
                    case 'unsetLink':
                        self.editor.chain().focus().unsetLink().run();
                        break;
                    case 'addImage':
                        const imgUrl = window.prompt('Bild URL');
                        self.editor.chain().focus().setImage({ href: imgUrl }).run();
                        break;
                    case 'horizontalRule':
                        self.editor.chain().focus().setHorizontalRule().run();
                        break;
                    case 'undo':
                        self.editor.chain().focus().undo().run();
                        break;
                    case 'redo':
                        self.editor.chain().focus().redo().run();
                        break;
                }
                self.clicked = true;
            },
        }
    });
