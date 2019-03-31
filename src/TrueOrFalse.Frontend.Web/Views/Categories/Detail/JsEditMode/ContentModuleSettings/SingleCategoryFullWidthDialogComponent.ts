class SingleCategoryFullWidthSettings {
    TemplateName: string = "";
    Name: string = "";
    CategoryId: number = 0;
    Description: string = "";
}

Vue.component('singlecategoryfullwidth-modal-component', {

    template: '#singlecategoryfullwidth-settings-dialog-template',

    singleCategoryFullWidthSettings: SingleCategoryFullWidthSettings,

    data() {
        return {
            newMarkdown: '',
            parentId: '',
            title: '',
            topicId: '',
            description: '',
        };
    },

    created() {
        var self = this;
        self.singleCategoryFullWidthSettings = new SingleCategoryFullWidthSettings();
    },

    watch: {
        newMarkdown: function() {
            this.settingsHasChanged = true;
        },
    },
    
    mounted: function() {
        $('#singlecategoryfullwidthSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#singlecategoryfullwidthSettingsDialog').data('parent').markdown;
                this.parentId = $('#singlecategoryfullwidthSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#singlecategoryfullwidthSettingsDialog').on('hidden.bs.modal',
            event => {
                if (!this.settingsHasChanged)
                    eventBus.$emit('close-content-module-settings-modal', false);
                this.clearData();
            });
    },

    methods: {
        clearData() {
            this.newMarkdown = '';
            this.parentId = '';
            this.title = '';
            this.topicId = '';
            this.description = '';

        },

        initializeData() {
            this.singleCategoryFullWidthSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);

            if (this.singleCategoryFullWidthSettings.Name)
                this.title = this.singleCategoryFullWidthSettings.Name;
            this.topicId = this.singleCategoryFullWidthSettings.CategoryId;
            if (this.singleCategoryFullWidthSettings.Description)
                this.description = this.singleCategoryFullWidthSettings.Description;
        },

        applyNewMarkdown() {

            this.singleCategoryFullWidthSettings.Name = this.title;
            this.singleCategoryFullWidthSettings.CategoryId = this.topicId;
            this.singleCategoryFullWidthSettings.Description = this.description;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.singleCategoryFullWidthSettings);
            Utils.ApplyMarkdown(this.newMarkdown, this.parentId);
            $('#singlecategoryfullwidthSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#singlecategoryfullwidthSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

