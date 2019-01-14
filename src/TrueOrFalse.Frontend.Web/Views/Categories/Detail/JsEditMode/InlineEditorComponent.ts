declare var Vue: any;
declare var marked: any;

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
                ],
                input: '# Hello World'
            }
            
        },
        computed: {
            compiledMarkdown: function () {
                return marked(this.input, { sanitize: true })
            }
        },
        methods: {
            update: _.debounce(function (e) {
                this.input = e.target.value
            }, 300)
        }
    }
);