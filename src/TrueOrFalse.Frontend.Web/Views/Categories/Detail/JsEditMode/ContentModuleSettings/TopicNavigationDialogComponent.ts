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
            parentId: '',
            title: '',
            text: '',
            load: 'Custom',
            order: '',
            topics: [],
            loadTopics: [],
            orderTopics: [],
            prevOrder: '',
            settingsHasChanged: false,
            newTopic: '',
            showTopicInput: false,
            topicOptions: {
                animation: 100,
                fallbackOnBody: true,
                filter: '.placeholder',
                preventOnFilter: false,
                onMove: this.onMove,
            },
            searchResults: '',
            searchType: 'Categories',
            options: []
        }
    },

    created() {
        var self = this;
        self.topicNavigationSettings = new TopicNavigationSettings();
    },

    mounted: function () {
        $('#topicnavigationSettingsDialog').on('show.bs.modal',
            event => { 
                this.topicNavigationSettings = $('#topicnavigationSettingsDialog').data('parent').moduleData;
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

        filteredSearch() {
            let results = [];

            if (this.searchResults)
                results = this.searchResults.Items.filter(i => i.Type === this.searchType);

            return results;
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
            this.load = 'Custom';
            this.order = '';
            this.newTopic = '';
            this.showTopicInput = false;
            this.loadTopics = [];
            this.orderTopics = [];
            this.prevOrder = '';
        },

        initializeData() {
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

        addTopic() {
            try {
                if (this.newTopic.Item.Id) {
                    this.topics.push(this.newTopic.Item.Id);
                    this.newTopic = '';
                }
            } catch (e) {
                $.post("/Logg/SetLoggReport",
                    {report: "Topic can't Added ---------- TopicNavigationDialogComponent.ts/addTopic " });
            }
        },

        hideTopicInput() {
            this.newTopic = '';
            this.showTopicInput = false;
        },

        removeTopic(index) {
            this.topics.splice(index, 1);
        },

        applyNewMarkdown() {
            if (this.title)
                this.topicNavigationSettings.Title = this.title;
            if (this.text)
                this.topicNavigationSettings.Text = this.text;

            const topicIdParts = $(".topicNavigationDialogData").map((idx, elem) => $(elem).attr("topicId")).get();
            if (topicIdParts.length > 0) {
                if (this.load != 'All')
                    this.topicNavigationSettings.Load = topicIdParts.join(',');
                else if (this.order == 'ManualSort')
                    this.topicNavigationSettings.Order = topicIdParts.join(',');
                else
                    this.topicNavigationSettings.Order = this.order;
            }

            Utils.ApplyContentModule(this.topicNavigationSettings, this.parentId);
            $('#topicnavigationSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#topicnavigationSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },

        onSearch(search, loading) {
            loading(true);
            this.search(loading, search, this);
        },
        search: _.debounce(function (loading, search, vm) {
            $.get("/Api/Search/ByName?term=" + search + "&type=" + this.searchType,
                (result) => {
                    this.searchResults = result;
                    vm.options = this.filteredSearch;
                    loading(false);
                }
            );
        }, 350),
    },
});

