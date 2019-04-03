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
            searchResults: '',
            searchType: 'Sets',
            options: [],
            selected: '',
        };
    },

    created() {
        var self = this;
        self.singleSetFullWidthSettings = new SingleCategoryFullWidthSettings();
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
            this.set = '';
            this.description = '';

        },

        initializeData() {
            this.singleSetFullWidthSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);

            if (this.singleSetFullWidthSettings.Title)
                this.title = this.singleSetFullWidthSettings.Title;
            if (this.singleSetFullWidthSettings.SetId)
                this.setId = this.singleSetFullWidthSettings.SetId;
            if (this.singleSetFullWidthSettings.Text)
                this.description = this.singleSetFullWidthSettings.Text;
        },

        applyNewMarkdown() {

            if (this.title)
                this.singleSetFullWidthSettings.Title = this.title;
            this.singleSetFullWidthSettings.SetId = this.selected.Item.Id;
            if (this.description)
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

