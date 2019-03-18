class SingleSetFullWidthSettings {
    TemplateName: string = "";
    Title: string = "";
    SetId: number = 0;
    Text: string = "";
}

Vue.component('singlesetfullwidth-modal-component', {

    template: '#singlesetfullwidth-settings-dialog-template',

    singleSetFullWidthSettings: SingleSetFullWidthSettings,

    data() {
        return {
            newMarkdown: '',
            parentId: '',
            title: '',
            setId: '',
            description: '',
        };
    },

    created() {
        var self = this;
        self.singleSetFullWidthSettings = new SingleCategoryFullWidthSettings();
    },

    watch: {
        newMarkdown: function() {
            this.settingsHasChanged = true;
        },
    },
    
    mounted: function() {
        $('#singlesetfullwidthSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#singlesetfullwidthSettingsDialog').data('parent').markdown;
                this.parentId = $('#singlesetfullwidthSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#singlesetfullwidthSettingsDialog').on('hidden.bs.modal',
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
            this.setId = '';
            this.description = '';

        },

        initializeData() {
            this.singleSetFullWidthSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);

            if (this.singleSetFullWidthSettings.Title)
                this.title = this.singleSetFullWidthSettings.Title;
            this.setId = this.singleSetFullWidthSettings.SetId;
            if (this.singleSetFullWidthSettings.Text)
                this.description = this.singleSetFullWidthSettings.Text;
        },

        applyNewMarkdown() {

            this.singleSetFullWidthSettings.Title = this.title;
            this.singleSetFullWidthSettings.SetId = this.setId;
            this.singleSetFullWidthSettings.Text = this.description;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.singleSetFullWidthSettings);
            Utils.ApplyMarkdown(this.newMarkdown, this.parentId);
            $('#singlesetfullwidthSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#singlesetfullwidthSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

