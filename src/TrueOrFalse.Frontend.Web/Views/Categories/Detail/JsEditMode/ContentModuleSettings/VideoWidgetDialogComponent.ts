class VideoWidgetSettings {
    TemplateName: string = "";
    SetId: number = 0;
}

Vue.component('videowidget-modal-component', {

    template: '#videowidget-settings-dialog-template',

    videoWidgetSettings: VideoWidgetSettings,

    data() {
        return {
            newMarkdown: '',
            parentId: '',
            setId: '',
        };
    },

    created() {
        var self = this;
        self.videoWidgetSettings = new VideoWidgetSettings();
    },

    watch: {
        newMarkdown: function() {
            this.settingsHasChanged = true;
        },
    },
    
    mounted: function() {
        $('#videowidgetSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#videowidgetSettingsDialog').data('parent').markdown;
                this.parentId = $('#videowidgetSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#svideowidgetSettingsDialog').on('hidden.bs.modal',
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
            this.setId = '';
        },

        initializeData() {
            this.videoWidgetSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
            this.setId = this.videoWidgetSettings.SetId;
        },

        applyNewMarkdown() {

            this.videoWidgetSettings.SetId = this.setId;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.videoWidgetSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            $('#videowidgetSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#videowidgetSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

