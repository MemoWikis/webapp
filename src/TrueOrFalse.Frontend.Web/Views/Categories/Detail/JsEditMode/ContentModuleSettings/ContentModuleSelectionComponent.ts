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
                'Cards',
                'CategoryNetwork',
                'ContentLists',
                'EducationOfferList',
                'InlineText',
                'MediaList',
                'SetCardMiniList',
                'SingleCategoryFullWidth',
                'SingleQuestionsQuiz',
                'SingleSetFullWidth',
                'Spacer',
                'TopicNavigation',
                'VideoWidget',
            ],
            selectedModule: '',
            modalType: '',
        };
    },

    created() {
        var self = this;
        self.contentModuleTemplate = new ContentModuleTemplate();
    },

    watch: {
        selectedModule: function(val) {
            if (val != 'inlinetext' &&
                val != 'spacer' &&
                val != 'medialist' &&
                val != 'contentlist' &&
                val != 'educationofferlist')
                this.modalType = '#' + this.selectedModule.toLowerCase() + 'SettingsDialog';
            else
                this.modalType = false;
        }
    },

    mounted: function() {
        $('#ContentModuleSelectionModal').on('show.bs.modal',
            event => {

            });

        $('#ContentModuleSelectionModal').on('hidden.bs.modal',
            event => {
                if (!this.settingsHasChanged)
                    eventBus.$emit('close-content-module-settings-modal', false);
                this.clearData();
            });
    },

    methods: {
        clearData() {
            this.templateMarkdown = '';
            this.selectedModule = '';
            this.modalType = '';
        },

        selectModule() {
            this.contentModuleTemplate.TemplateName = this.selectedModule;
            this.templateMarkdown = Utils.ConvertJsonToMarkdown(this.contentModuleTemplate);
            let template = {
                id: 'noParentId',
                markdown: this.templateMarkdown,
            };
            $('#ContentModuleSelectionModal').modal('hide');

            if (this.modalType)
                $(this.modalType).data('parent', template).modal('show');
        },

        closeModal() {
            $('#ContentModuleSelectionModal').modal('hide');
        },
    },
});

