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
            newSet: '',
            searchResults: '',
            searchType: 'Sets',
            options: [],
        };
    },

    created() {
        var self = this;
        self.videoWidgetSettings = new VideoWidgetSettings();
    },

    computed: {
        filteredSearch() {
            let results = [];

            if (this.searchResults)
                results = this.searchResults.Items.filter(i => i.Type === this.searchType);

            return results;
        },
    },

    watch: {
        newMarkdown: function() {
            this.settingsHasChanged = true;
        },

        newSet: function () {
            try {
                this.setId = this.newSet.Item.Id;
            } catch (e) {};
        }
    },
    
    mounted: function() {
        $('#videowidgetSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#videowidgetSettingsDialog').data('parent').markdown;
                this.parentId = $('#videowidgetSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#videowidgetSettingsDialog').on('hidden.bs.modal',
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
            Utils.ApplyMarkdown(this.newMarkdown, this.parentId);
            $('#videowidgetSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#videowidgetSettingsDialog').modal('hide');
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

