declare var postscribe: any;

Vue.component('content-module-widget', {
    props: {
        widgetType: String,
        widgetId: String,
        src: String,
        dataT: String,
        dataId: String,
        dataWidth: String,
        dataMaxwidth: String,
        dataLogoon: String,
        dataHideKnowledgeBtn: String,
    },

    template: '<div :id="widgetId"></div>',

    data() {
        return {
        };
    },
    
    mounted() {
        var el = '#' + this.widgetId;
        let script;

        if (this.widgetType == 'video') {
            script = '<\script src="' + this.src + '" data-t="' + this.dataT + '" data-id="' + this.dataId + '" data-width="' + this.dataWidth + '"></\script>';
        };

        if (this.widgetType == 'quiz') {
            script = '<\script src="' + this.src + '" data-t="' + this.dataT + '" data-id="' + this.dataId + '" data-width="' + this.dataWidth + '" data-maxwidth="' + this.dataMaxwidth + '" data-logoon="' + this.dataLogoon + '" data-hideKnowledgeBtn="' + this.dataHideKnowledgeBtn + '"></\script>';
        };

        postscribe(el, script, {
            error: function (e) {
                console.log(e);
            }
        });
    },
});

Vue.component('add-content-module', {
    template: '#add-content-module-template',

    data() {
        return {
            type: '',
        };
    },

});

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
            widgetId: '',
        };
    },

    created() {
        if (this.contentModuleType != "inlinetext") {
            this.modalType = '#' + this.contentModuleType + 'SettingsDialog';
        }
        this.id = this.contentModuleType + 'Module-' + (this._uid + Math.floor((Math.random() * 10000) + 1));
        if (this.contentModuleType == 'videowidget' || this.contentModuleType == 'singlequestionsquiz')
            this.widgetId = 'widget' + (this._uid + Math.floor((Math.random() * 2000) + 1));
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

    computed: {
        missingText: function() {
            if (this.contentModuleType == 'inlinetext' && this.canBeEdited) {
                let trimmedMarkdown = this.markdown.trim().replace(' ', '');
                if (trimmedMarkdown.length > 0)
                    return false;
                else
                    return true;
            }
        },
    },

    watch: {
        canBeEdited: function (val) {
            if (val) {
                if (this.contentModuleType != 'inlinetext') {
                }
                this.dataTarget = this.modalType;
                this.markdown = this.origMarkdown;
                if (!this.contentModuleType)
                    this.isDeleted = true;
            } else {
                this.markdown = '';
                this.dataTarget = '';
                if (!this.contentModuleType)
                    this.isDeleted = false;
            };
        },
    },

    methods: {

        updateHoverState(isHover) {
            if (this.contentModuleType) {
                const self = this;
                if (self.canBeEdited || this.contentModuleType == 'AddModuleButton') {
                    self.hoverState = isHover;
                }
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

        addModule() {
            $('#ContentModuleSelectionModal').modal('show');
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