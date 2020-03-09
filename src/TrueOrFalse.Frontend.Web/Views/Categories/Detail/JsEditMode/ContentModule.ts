declare var postscribe: any;

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
            questions: [],
            baseTitle: '',
            baseDescription: '',
            readyToFocus: true,
            textAreaId: '',
            moveOptionsToCenter: '',
        };
    },

    created() {
        if (this.contentModuleType != "inlinetext") {
            this.modalType = '#' + this.contentModuleType + 'SettingsDialog';
        };
        this.id = this.contentModuleType + 'Module-' + (this._uid + Math.floor((Math.random() * 10000) + 1));
        if (this.contentModuleType == 'videowidget' || this.contentModuleType == 'singlequestionsquiz')
            this.widgetId = 'widget' + (this._uid + Math.floor((Math.random() * 2000) + 1));
        if (this.contentModuleType == 'singlequestionsquiz') {
            this.singleQuestionsQuizSettings = Utils.ConvertEncodedHtmlToJson(this.origMarkdown);
            if (this.singleQuestionsQuizSettings.QuestionIds)
                this.questions = this.singleQuestionsQuizSettings.QuestionIds.split(',');
        };
        if (this.contentModuleType == 'AddModuleButton')
            this.id = 'ContentModulePlaceholder';
        this.textAreaId = 'TextArea-' + (this._uid + Math.floor((Math.random() * 100) + 1));
    },

    mounted() {
        eventBus.$on('set-edit-mode', state => this.canBeEdited = state);
        eventBus.$on('close-content-module-settings-modal',
            (event) => {
                if (this.isListening && event) {
                    this.isDeleted = true;
                    this.isListening = false;
                } else if (this.isListening && !event) {
                    this.isListening = false;
                };
            });

        eventBus.$on('set-hover-state',
            (state) => {
                if (state == false) {
                    this.readyToFocus = false;
                    this.hoverState = false;
                };
            });

        eventBus.$on('set-new-content-module',
            (state) => {
                if (state == true && this.readyToFocus == true) {
                    this.isListening = false;
                    this.hoverState = true;
                    if (this.contentModuleType == 'inlinetext') {
                        this.textCanBeEdited = true;
                    };
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
            };
        },
    },

    watch: {
        canBeEdited: function(val) {
            if (val) {
                this.dataTarget = this.modalType;
                this.markdown = this.origMarkdown;
                if (!this.contentModuleType) // hides default modules on non customised pages
                    this.isDeleted = true;
            } else {
                this.markdown = '';
                this.dataTarget = '';
                if (!this.contentModuleType)
                    this.isDeleted = false;
                this.textCanBeEdited = false;
                this.hoverState = false;
            };
        },
    },

    updated: function() {
        if ((this.$el.clientHeight) < 80)
            this.moveOptionsToCenter = true;
        else
            this.moveOptionsToCenter = false;
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

        addModule(val) {
            let data = {
                id: this.id,
                position: val,
            }
            $('#ContentModuleSelectionModal').data('data', data).modal('show');
        },

        editInlineText() {
            if (this.canBeEdited) {
                eventBus.$emit('set-hover-state', false);

                this.textCanBeEdited = true;
            };
        },

        moveUp() {
            var currentId = '#' + this.id;
            var currentDiv = $(currentId);
            if (currentDiv.prev().attr('class') == 'DescriptionSection')
                return;
            else
                currentDiv.prev().before(currentDiv);
        },

        moveDown() {
            var currentId = '#' + this.id;
            var currentDiv = $(currentId);
            if (currentDiv.next().attr('id') == 'ContentModulePlaceholder')
                return;
            else
                currentDiv.next().after(currentDiv);
        },

        setBaseTitle(val) {
            this.baseTitle = val;
        },
    },
});