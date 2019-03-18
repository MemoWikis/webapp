var contentModuleComponent = Vue.component('content-module', {

    props: {
        origMarkdown: String,
        contentModuleType: String,
    },

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            markdown: '',
            isListening: false,
            modalType: '',
            id: '',
            textCanBeEdited: false,
            dataTarget: '',
            dataId: '',
        };
    },

    ready() {
        if (this.contentModuleType == 'videowidget' || this.contentModuleType == 'singlequestionsquiz') {
            let scriptEl = document.createElement('script');

            if (this.contentModuleType == 'videowidget') {
                scriptEl.setAttribute('data-t', 'setVideo');
            } else if (this.contentModuleType == 'singlequestionsquiz') {
                scriptEl.setAttribute('data-t', 'question');
                scriptEl.setAttribute('data-maxwidth', '100%');
                scriptEl.setAttribute('data-logoon', 'false');
                scriptEl.setAttribute('data-hideKnowledgeBtn', 'true');
            }

            scriptEl.setAttribute('src', 'https://memucho.de/views/widgets/w.js');
            scriptEl.setAttribute('data-width', '100%');
            scriptEl.setAttribute('data-id', this.dataId);


            this.$refs.scriptWidget.appendChild(scriptEl);
        }

    },

    created() {
        if (this.contentModuleType != "inlinetext") {
            this.modalType = '#' + this.contentModuleType + 'SettingsDialog';
        }
        this.id = this.contentModuleType + 'Module-' + (this._uid - 2);

        let ckeditor = document.createElement('script'); ckeditor.setAttribute('src', "//cdn.ckeditor.com/4.6.2/full/ckeditor.js");
        document.head.appendChild(ckeditor);
    },

    mounted() {
        eventBus.$on('set-edit-mode', state => this.canBeEdited = state);
        eventBus.$on('close-content-module-settings-modal', (event) => {
            if (this.isListening && event) {
                this.isDeleted = true;
                this.isListening = false;
            } else if (this.isListening && !event) {
                this.isListening = false;
            };
        });
    },

    watch: {
        canBeEdited: function (val) {
            if (val) {
                if (this.contentModuleType != 'inlinetext') {
                }
                this.dataTarget = this.modalType;
                this.markdown = this.origMarkdown;
            } else {
                this.markdown = '';
                this.dataTarget = '';
            };
        },
    },

    methods: {

        updateHoverState(isHover) {
            const self = this;
            if (self.canBeEdited) {
                self.hoverState = isHover;
            }
        },

        deleteModule() {
            const self = this;
            self.isDeleted = true;
        },

        editModule() {
            if (this.canBeEdited) {
                if (this.contentModuleType != 'inlineText') {
                    this.isListening = true;
                    let parent = {
                        id: this.id,
                        markdown: this.markdown,
                    };
                    $(this.modalType).data('parent', parent).modal('show');
                };
            }
        },

        editInlineText() {
            if (this.canBeEdited) {
                this.textCanBeEdited = true;
            };
        },

        moveUp() {

        },

        moveDown() {

        },
    },
});