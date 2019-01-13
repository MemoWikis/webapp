declare var Vue: any;
//declare var tiptap: any;
//declare var extensions: any;


Vue.component('inline-editor-component',
    {
        data() {
            return {
                editOffset: -1,
                editPost: {},
                editPostOri: {},
                text: [
                    { article: 'A', id: 1 },
                    { article: 'B', id: 2 }
                ]
            }
            
        },
//        methods: {
//            startEditing(index) {
//                this.editOffset = index;
//                this.editPost = this.posts[index];
//                this.editPostOri = JSON.parse(JSON.stringify(this.editPost));
//                this.$nextTick(function() {
//                    console.log('item-article-' + this.editOffset)
//                    document.getElementById('item-article-' + this.editOffset).focus()
//                }.bind(this))
//            },
//            updatePost() {
//                this.editOffset = -1
//                this.editPostOri = {}
//                this.editPost = {}
//            },
//            cancelEditing() {
//                this.$set(this.posts, this.editOffset, this.editPostOri);
//                this.editOffset = -1;
//                this.editPostOri = {}
//                this.editPost = {}
//            }
//        }
    });

//            tiptap: {
//                components: {
//                    EditorContent: tiptap.EditorContent,
//                    EditorMenuBar: tiptap.EditorMenuBar
//                },
//                data() {
//                    return {
//                        editor: new tiptap.Editor({
//                            extensions: [
//                                new extensions.Blockquote(),
//                                new extensions.BulletList(),
//                                new extensions.CodeBlock(),
//                                new extensions.HardBreak(),
//                                new extensions.Heading({ levels: [1, 2, 3] }),
//                                new extensions.ListItem(),
//                                new extensions.OrderedList(),
//                                new extensions.TodoItem(),
//                                new extensions.TodoList(),
//                                new extensions.Bold(),
//                                new extensions.Code(),
//                                new extensions.Italic(),
//                                new extensions.Link(),
//                                new extensions.Strike(),
//                                new extensions.Underline(),
//                                new History(),
//                            ],
//                            content: `
//                      <h2>
//                        Hi there,
//                      </h2>
//                      <p>
//                        this is a very <em>basic</em> example of tiptap.
//                      </p>
//                      <pre><code>body { display: none; }</code></pre>
//                      <ul>
//                        <li>
//                          A regular list
//                        </li>
//                        <li>
//                          With regular items
//                        </li>
//                      </ul>
//                      <blockquote>
//                        It's amazing 👏
//                        <br />
//                        – mom
//                      </blockquote>
//                    `,
//                        }),
//                    }
//                },
//                beforeDestroy() {
//                    this.editor.destroy();
//                },
//            },

//    });