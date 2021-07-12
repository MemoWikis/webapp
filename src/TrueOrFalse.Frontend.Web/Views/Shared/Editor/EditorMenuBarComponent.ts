var menuBar = Vue.component('editor-menu-bar-component',
    {
        props: ['editor'],
        template: '#editor-menu-bar-template',
        data() {
            return {
                focused: false,
            }
        },
        mounted() {
            if (this.editor) {
                this.editor.on('focus', () => this.focused = true);
                this.editor.on('blur', () => this.focused = false);
            }

        },
        methods: {
            setLink() {
                const url = window.prompt('URL');

                this.editor
                    .chain()
                    .focus()
                    .extendMarkRange('link')
                    .setLink({ href: url })
                    .run();
            },
        }
    })
