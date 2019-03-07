class TopicNavigationSettings {
    TemplateName: string = "";
    Title: string = "";
    Text: string = "";
    Load: string = "";
    Order: string = "";
}

Vue.component('topicnavigation-modal-component', {
    props: ['origMarkdown'],

    template: '#topicnavigation-settings-dialog-template',

    topicNavigationSettings: TopicNavigationSettings,

    data() {
        return {
            newMarkdown: '',
            parentId: '',
            title: '',
            text: '',
            load: '',
            order: '',
            sets: [],
            settingsHasChanged: false,
        }
    },

    created() {
        var self = this;
        self.topicNavigationSettings = new TopicNavigationSettings();
    },

    mounted: function () {
        $('#topicnavigationSettingsDialog').on('show.bs.modal',
            event => {
                this.settingsHasChanged = false;
                this.newMarkdown = $('#topicnavigationSettingsDialog').data('parent').markdown;
                this.parentId = $('#topicnavigationSettingsDialog').data('parent').id;
                this. topicNavigationSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
            });

        $('#topicnavigationSettingsDialog').on('hidden.bs.modal', function () {
            this.sets = [];
            if (!this.settingsHasChanged)
                eventBus.$emit('close-content-module-settings-modal', this.preview);
        });
    },

    watch: {
        newMarkdown: function () {
            if (!this.settingsHasChanged)
                eventBus.$emit('close-content-module-settings-modal', false);
        },
    },

    methods: {
        addSet(val) {
            this.sets.push(val);
        },
        removeSet(index) {
            this.sets.splice(index, 1);
        },

        applyNewMarkdown() {
            const setIdParts = $(".topicNavigationSettings").map((idx, elem) => $(elem).attr("setId")).get();
            if (this.order == 'free' && setIdParts.length >= 1)
                this.topicNavigationSettings.SetListIds = setIdParts.join(',');
            this.topicNavigationSettings.Title = this.title;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.topicNavigationSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            this.hasPreview = true;
            $('#topicnavigationSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#topicnavigationSettingsDialog').modal('hide');
        },
    },
});

