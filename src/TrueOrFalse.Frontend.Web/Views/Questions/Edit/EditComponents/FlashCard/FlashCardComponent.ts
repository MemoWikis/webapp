var {
    tiptap,
    tiptapUtils,
    tiptapCommands,
    tiptapExtensions,
} = tiptapBuild;
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
Vue.component('editor-content', tiptap.EditorContent);

Vue.component('flashcard-component', {
        props: ['current-category-id'],
        data() {
            return {
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
                    onUpdate: ({ getJSON, getHTML }) => {
                        this.answerJson = getJSON();
                        this.answerHtml = getHTML();
                    },
                }),
                answer: null,
                answerJson: null,
                answerHtml: null,
            }
        },

        methods: {
        }
    })