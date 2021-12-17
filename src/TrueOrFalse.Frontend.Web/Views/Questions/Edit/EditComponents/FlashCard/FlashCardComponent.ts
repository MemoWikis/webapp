Vue.component('flashcard-component', {
    props: ['solution', 'highlightEmptyFields'],
    template: '#flashcard-template',
    data() {
        return {
            content: null,
            answerEditor: new tiptapEditor({
                editable: true,
                extensions: [
                    tiptapStarterKit,
                    tiptapLink.configure({
                        HTMLAttributes: {
                            target: '_self',
                            rel: 'noopener noreferrer nofollow'
                        }
                    }),
                    tiptapCodeBlockLowlight.configure({
                        lowlight,
                    }),
                    tiptapUnderline,
                    tiptapPlaceholder.configure({
                        emptyEditorClass: 'is-editor-empty',
                        emptyNodeClass: 'is-empty',
                        placeholder: 'Rückseite der Karteikarte',
                        showOnlyCurrent: true,
                    }),
                    tiptapImage
                ],
                content: this.content,
                onUpdate: ({ editor }) => {
                    this.$parent.flashCardAnswer = editor.getHTML();
                    this.$parent.solutionIsValid = this.answerEditor.state.doc.textContent.length > 0;
                },
                editorProps: {
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
                }
            }),
            answerJson: null,
            answerHtml: null,
        }
    },

    mounted() {
        eventBus.$on('clear-flashcard', () => this.answerEditor.commands.setContent(''));
        if (this.solution) {
            this.answerEditor.commands.setContent(this.solution);
            this.$parent.flashCardAnswer = this.solution;
            this.$parent.solutionIsValid = this.answerEditor.state.doc.textContent.length > 0;
        }
    },

    methods: {
    }
})