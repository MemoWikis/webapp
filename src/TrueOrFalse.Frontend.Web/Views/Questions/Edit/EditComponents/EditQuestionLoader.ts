declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var EditQuestionLoader = new Vue({
    name: 'EditQuestionLoader',
    el: '#EditQuestionLoaderApp',
    data() {
        return {
            tiptapIsReady: false,
            modalIsReady: false,
            editQuestionComponentLoaded: false,
        }
    },
    mounted() {
        if (typeof (tiptapEditor) !== 'undefined' && tiptapEditor != null)
            this.tiptapIsReady = true;
        eventBus.$on('tiptap-is-ready', (val) => this.$nextTick(() => this.tiptapIsReady = val));
        eventBus.$on('edit-question-is-ready', (val) => this.$nextTick(() => this.modalIsReady = val));
        eventBus.$on('edit-question-component-loaded', (val) => this.$nextTick(() => this.editQuestionComponentLoaded = val));
        eventBus.$on('open-edit-question-modal',
            e => {
                var question = {
                    questionId: e.questionId,
                    edit: e.edit,
                    sessionIndex: e.sessionIndex,
                    categoryId: e.categoryId,
                    questionHtml: e.questionHtml,
                    solution: e.solution
                };
                if (this.modalIsReady && this.tiptapIsReady)
                    $('#EditQuestionModal').data('question', question).modal({ backdrop: 'static', keyboard: false });
                else if (!this.modalIsReady && this.tiptapIsReady)
                    this.loadEditor(question);
                else if (!this.tiptapIsReady)
                    this.loadTiptap(question);
            });
    },
    methods: {
        loadEditor(question = null) {
            var self = this;
            $.ajax({
                type: 'get',
                url: '/EditQuestion/GetEditQuestionModal/',
                success(html) {
                    $(html).insertBefore('#EditQuestionLoaderApp');
                    if (question != null)
                        self.$nextTick(() => {
                            self.loadModal(question);
                        });
                },
            });
        },
        loadModal(question) {
            this.$nextTick(() => {
                $('#EditQuestionModal').data('question', question).modal('show');
            });
        },
        loadTiptap(question) {
            var self = this;
            $.ajax({
                type: 'get',
                url: '/EditCategory/GetTiptap/',
                success(html) {
                    $(html).insertAfter('script#pin-category-template');
                    if (!self.modalIsReady)
                        self.loadEditor(question);
                    else
                        self.loadModal(question);
                },
            });
        },
    }
});