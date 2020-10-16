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
            if (val != 'InlineText')
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

        eventBus.$on('add-inline-text-module',
            () => {
                let template = {
                    id: 'before:ContentModulePlaceholder',
                    moduleData: {
                        TemplateName: 'InlineText'
                    },
                };
                Utils.ApplyContentModule(template.moduleData, template.id);
            });
    },

    methods: {
        setActive(val) {
            this.selectedModule = val;
            if (val != 'InlineText')
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
            let template = {
                id: this.modulePosition + ':' + this.moduleId,
                moduleData: this.contentModuleTemplate,
            };
            $('#ContentModuleSelectionModal').modal('hide');

            eventBus.$emit('set-hover-state', false);

            if (this.modalType)
                $(this.modalType).data('parent', template).modal('show');
            else
                Utils.ApplyContentModule(template.moduleData, template.id);
        },

        closeModal() {
            $('#ContentModuleSelectionModal').modal('hide');
        },
    },
});

