class ContentModuleTemplate {
    TemplateName: string = "";
};

Vue.component('content-module-selection-modal-component', {
    template: '#content-module-selection-dialog-template',

    contentModuleTemplate: ContentModuleTemplate,

    data() {
        return {
            templateMarkdown: '',
            contentModules: [
                { type: 'InlineText', group: 'main', name: 'Text', tooltip: 'Freie Text Gestaltung per Markdown.' },
                { type: 'TopicNavigation', group: 'main', name: 'Themenliste', tooltip: 'Zeigt eine Liste aller Hauptthemen.' },
                { type: 'CategoryNetwork', group: 'main', name: 'Themennetzwerk', tooltip: 'Über- und untergeordnete Themen werden übersichtlich dargestellt.' },
            ],
            selectedModule: '',
            modalType: '',
            modulePosition: '',
            moduleId: '',
            showMainModules: true,
            showSubModules: true,
            showMiscModules: true,
        };
    },

    created() {
        var self = this;
        self.contentModuleTemplate = new ContentModuleTemplate();
        if (this.mainModules.length == 0)
            this.showMainModules = false;
        if (this.subModules.length == 0)
            this.showSubModules = false;
        if (this.miscModules.length == 0)
            this.showMiscModules = false;
    },

    computed: {
        mainModules: function() {
            return this.contentModules.filter(function (i) {
                return i.group === 'main';
            });
        },

        subModules: function () {
            return this.contentModules.filter(function (i) {
                return i.group === 'sub';
            });
        },

        miscModules: function () {
            return this.contentModules.filter(function (i) {
                return i.group === 'misc';
            });
        },
    },

    watch: {
        selectedModule: function(val) {
            if (val != 'InlineText' &&
                val != 'Spacer' &&
                val != 'MediaList' &&
                val != 'ContentLists' &&
                val != 'RelatedContentLists' &&
                val != 'EducationOfferList' &&
                val != 'CategoryNetwork')
                this.modalType = '#' + this.selectedModule.toLowerCase() + 'SettingsDialog';
            else
                this.modalType = false;
        }
    },

    mounted: function() {
        $('#ContentModuleSelectionModal').on('show.bs.modal',
            event => {
                this.modulePosition = $('#ContentModuleSelectionModal').data('data').position;
                this.moduleId = $('#ContentModuleSelectionModal').data('data').id;
            });

        $('#ContentModuleSelectionModal').on('hidden.bs.modal',
            event => {
                if (!this.settingsHasChanged)
                    eventBus.$emit('close-content-module-settings-modal', false);
                this.clearData();
            });
    },

    methods: {
        setActive(val) {
            this.selectedModule = val;
            if (val != 'InlineText' &&
                val != 'Spacer' &&
                val != 'MediaList' &&
                val != 'ContentLists' &&
                val != 'RelatedContentLists' &&
                val != 'EducationOfferList' &&
                val != 'CategoryNetwork')
                this.modalType = '#' + this.selectedModule.toLowerCase() + 'SettingsDialog';
            else
                this.modalType = false;
            this.selectModule();
        },

        clearData() {
            this.templateMarkdown = '';
            this.selectedModule = '';
            this.modalType = '';
            this.modulePosition = '';

        },

        selectModule() {
            this.contentModuleTemplate.TemplateName = this.selectedModule;
            this.templateMarkdown = Utils.ConvertJsonToMarkdown(this.contentModuleTemplate);
            let template = {
                id: this.modulePosition + ':' + this.moduleId,
                markdown: this.templateMarkdown,
            };
            $('#ContentModuleSelectionModal').modal('hide');

            eventBus.$emit('set-hover-state', false);

            if (this.modalType)
                $(this.modalType).data('parent', template).modal('show');
            else if (this.selectedModule == 'InlineText') {
                Utils.ApplyMarkdown('', template.id);
            }
            else
                Utils.ApplyMarkdown(template.markdown, template.id);
        },

        closeModal() {
            $('#ContentModuleSelectionModal').modal('hide');
        },
    },
});

