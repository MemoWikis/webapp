class TopicNavigationSettings {
    TemplateName: string = "";
    Title: string = "";
    Text: string = "";
    Load: string = "";
    Order: string = "";
}

Vue.component('topicnavigation-modal-component', {

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
            topics: [],
            loadTopics: [],
            orderTopics: [],
            prevOrder: '',
            settingsHasChanged: false,
            newTopicId: '',
            showTopicInput: false,
            topicOptions: {
                animation: 100,
                fallbackOnBody: true,
                filter: '.placeholder',
                preventOnFilter: false,
                onMove: this.onMove,
            },
        }
    },

    created() {
        var self = this;
        self.topicNavigationSettings = new TopicNavigationSettings();
    },

    mounted: function () {
        $('#topicnavigationSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#topicnavigationSettingsDialog').data('parent').markdown;
                this.parentId = $('#topicnavigationSettingsDialog').data('parent').id;
                this.initializeData();

            });

        $('#topicnavigationSettingsDialog').on('hidden.bs.modal',
            event => {
            if (!this.settingsHasChanged)
                eventBus.$emit('close-content-module-settings-modal', false);
            this.clearData();
        });
    },

    computed: {
        showTopicList: function () {
            if (this.load == 'All' && this.order == 'ManualSort') {
                this.topics = this.orderTopics;
                return true;
            } else if (this.load != 'All') {
                this.topics = this.loadTopics;
                return true;
            }
        },
    },

    watch: {
        newMarkdown: function () {
            this.settingsHasChanged = true;
        },
        order: function(newVal, oldVal) {
            this.prevOrder = oldVal;
        },
        load: function() {
            if (this.order == 'ManualSort')
                this.order = this.prevOrder;
            this.showTopicInput = false;
        },
    },

    methods: {
        clearData() {
            this.newMarkdown = '';
            this.parentId = '';
            this.topics = [];
            this.settingsHasChanged = false;
            this.title = '';
            this.text = '';
            this.load = '';
            this.order = '';
            this.newTopicId = '';
            this.showTopicInput = false;
            this.loadTopics = [];
            this.orderTopics = [];
            this.prevOrder = '';
        },

        initializeData() {
            this.topicNavigationSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);

            if (this.topicNavigationSettings.Title)
                this.title = this.topicNavigationSettings.Title;
            if (this.topicNavigationSettings.Text)
                this.text = this.topicNavigationSettings.Text;
            if (this.topicNavigationSettings.Load) {
                if (this.topicNavigationSettings.Load != 'All') {
                    this.loadTopics = this.topicNavigationSettings.Load.split(',');
                    this.load = 'Custom';
                } else {
                    this.load = this.topicNavigationSettings.Load;
                };
            }

            if (this.topicNavigationSettings.Order) {
                if (this.topicNavigationSettings.Order != 'Name' &&
                    this.topicNavigationSettings.Order != 'QuestionAmount') {
                    this.order = 'ManualSort';
                    this.orderTopics = this.topicNavigationSettings.Order.split(',');
                } else {
                    this.order = this.topicNavigationSettings.Order;
                };
            }
        },

        addTopic(val) {
            this.topics.push(val);
            this.newTopicId = '';
        },

        hideTopicInput() {
            this.newTopicId = '';
            this.showTopicInput = false;
        },

        removeTopic(index) {
            this.topics.splice(index, 1);
        },

        applyNewMarkdown() {
            
            this.topicNavigationSettings.Title = this.title;
            this.topicNavigationSettings.Text = this.text;

            const topicIdParts = $(".topicNavigationDialogData").map((idx, elem) => $(elem).attr("topicId")).get();
            if (topicIdParts.length >= 1) {
                if (this.load != 'All')
                    this.topicNavigationSettings.Load = topicIdParts.join(',');
                if (this.order == 'ManualSort')
                    this.topicNavigationSettings.Order = topicIdParts.join(',');
                else
                    this.topicNavigationSettings.Order = this.order;
            }

            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.topicNavigationSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            $('#topicnavigationSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#topicnavigationSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

