﻿
Vue.component('flashcard-component', {
    props: ['solution'],
        data() {
            return {
                content: null,
                answerEditor: new tiptap.Editor({
                    editable: true,
                    extensions: [
                        new tiptapExtensions.Blockquote(),
                        new tiptapExtensions.BulletList(),
                        new tiptapExtensions.CodeBlock(),
                        new tiptapExtensions.HardBreak(),
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
                        //new tiptapExtensions.CodeBlockHighlight({
                        //    languages: {
                        //        apache,
                        //        //cLike,
                        //        xml,
                        //        bash,
                        //        //c,
                        //        coffeescript,
                        //        csharp,
                        //        css,
                        //        markdown,
                        //        diff,
                        //        ruby,
                        //        go,
                        //        http,
                        //        ini,
                        //        java,
                        //        javascript,
                        //        json,
                        //        kotlin,
                        //        less,
                        //        lua,
                        //        makefile,
                        //        perl,
                        //        nginx,
                        //        objectivec,
                        //        php,
                        //        phpTemplate,
                        //        plaintext,
                        //        properties,
                        //        python,
                        //        pythonREPL,
                        //        rust,
                        //        scss,
                        //        shell,
                        //        sql,
                        //        swift,
                        //        yaml,
                        //        typescript,
                        //    },
                        //}),
                        new tiptapExtensions.Placeholder({
                            emptyEditorClass: 'is-editor-empty',
                            emptyNodeClass: 'is-empty',
                            emptyNodeText: 'Rückseite der Karteikarte',
                            showOnlyCurrent: true,
                        })
                    ],
                    content: this.content,
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.$parent.flashCardAnswer = getHTML();
                    },
                }),
                answerJson: null,
                answerHtml: null,
            }
    },

    mounted() {
        if (this.solution) {
            let content = JSON.parse(this.solution).Text;
            this.answerEditor.setContent(content);
        }

    },

    methods: {
    }
})