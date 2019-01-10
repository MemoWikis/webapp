var _tiptap = { tiptap }
var _tiptapExtensions = { extensions }

Vue.component('inline-editor-component',
    {
        

        exports.default = {
        components: {
            EditorContent: _tiptap.EditorContent,
            EditorMenuBar: _tiptap.EditorMenuBar,
            Icon: _Icon2.default
        },

        import Icon from 'Components/Icon',
        import { Editor, EditorContent, EditorMenuBar } from 'tiptap',
        import {
            Blockquote,
            CodeBlock,
            HardBreak,
            Heading,
            OrderedList,
            BulletList,
            ListItem,
            TodoItem,
            TodoList,
            Bold,
            Code,
            Italic,
            Link,
            Strike,
            Underline,
            History,
                } from 'tiptap-extensions'
            export default {
                components: {
                    EditorContent,
                    EditorMenuBar,
                    Icon,
                },
                data() {
                    return {
                        editor: new Editor({
                            extensions: [
                                new Blockquote(),
                                new BulletList(),
                                new CodeBlock(),
                                new HardBreak(),
                                new Heading({ levels: [1, 2, 3] }),
                                new ListItem(),
                                new OrderedList(),
                                new TodoItem(),
                                new TodoList(),
                                new Bold(),
                                new Code(),
                                new Italic(),
                                new Link(),
                                new Strike(),
                                new Underline(),
                                new History(),
                            ],
                            content: `
                      <h2>
                        Hi there,
                      </h2>
                      <p>
                        this is a very <em>basic</em> example of tiptap.
                      </p>
                      <pre><code>body { display: none; }</code></pre>
                      <ul>
                        <li>
                          A regular list
                        </li>
                        <li>
                          With regular items
                        </li>
                      </ul>
                      <blockquote>
                        It's amazing 👏
                        <br />
                        – mom
                      </blockquote>
                    `,
                        }),
                    }
                },
                beforeDestroy() {
                    this.editor.destroy()
                },
            }

    })