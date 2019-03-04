class TopicNavigationSettings {
    Title: string = "";
    Text: string = "";
    Load: string = "";
    Order: string = "";
}

Vue.component('topicnavigation-modal-component', {
    props: ['origMarkdown'],

    template: '#topicnavigation-settings-dialog-template',

    _topicNavigationSettings: TopicNavigationSettings,

    data() {
        return {
            newMarkdown: '',
            sets: [],
            parentId: '',
        }
    },

    created() {
        var self = this;
        self._topicNavigationSettings = new TopicNavigationSettings();
    },

    mounted: function () {
        $('#topicnavigationSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = event.relatedTarget.getAttribute('data-markdown');
                this.parentId = event.relatedTarget.getAttribute('data-component-id');
                this. _topicNavigationSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
            });

        $('#topicnavigationSettingsDialog').on('hidden.bs.modal', function () {
            this.preview = false;
        });
    },

    methods: {
        applyNewMarkdown() {
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this._topicNavigationSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            $('#topicnavigationSettingsDialog').modal('hide');
        },
    },
});

