declare var postscribe: any;

var contentModuleComponent = Vue.component('content-module', {
    props: {
        origContent: String,
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
            content: null,
            uid: null,
        };
    },

    created() {
        if (this.contentModuleType != "inlinetext") {
            this.modalType = '#' + this.contentModuleType + 'SettingsDialog';
            if (this.origContent != null)
                this.content = JSON.parse(this.origContent);
        };
        this.uid = this._uid + Math.floor((Math.random() * 10000) + 1);
        this.id = this.contentModuleType + 'Module-' + (this.uid);
        this.id = this.contentModuleType + 'Module-' + (this.uid);
        if (this.contentModuleType == 'AddModuleButton')
            this.id = 'ContentModulePlaceholder';
        this.textAreaId = 'TextArea-' + (this._uid + Math.floor((Math.random() * 100) + 1));
        if (this.contentModuleType == "topicnavigation")
            eventBus.$emit('content-change');
    },

    mounted() {
        eventBus.$on('set-edit-mode', (state) => {
            this.canBeEdited = state;
            this.updateModule();
        });
        eventBus.$on('close-content-module-settings-modal',
            (event) => {
                if (this.isListening && event) {
                    this.isDeleted = true;
                    this.isListening = false;
                } else if (this.isListening && !event) {
                    this.isListening = false;
                };
            });
    },

    watch: {
        canBeEdited(val) {
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
        content() {
            this.updateModule();
        }
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
            eventBus.$emit('content-change');
            const self = this;
            self.isDeleted = true;
        },

        updateModule() {
            if (this.contentModuleType == "topicnavigation" || this.contentModuleType == "inlinetext") {
                let module = {
                    id: this.uid,
                    contentData: this.content
                };
                eventBus.$emit('get-module', module);
            }
        },

        editModule() {
            if (this.canBeEdited) {
                if (this.contentModuleType != 'inlineText') {
                    this.isListening = true;
                    let parent = {
                        id: this.id,
                        moduleData: this.content,
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