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
            searchResults: '',
            searchType: 'Categories',
            options: [],
            selected: '',
        };
    },

    created() {
        var self = this;
        self.singleCategoryFullWidthSettings = new SingleCategoryFullWidthSettings();
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
        $('#singlecategoryfullwidthSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#singlecategoryfullwidthSettingsDialog').data('parent').markdown;
                this.parentId = $('#singlecategoryfullwidthSettingsDialog').data('parent').id;
                console.log(event);
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
            this.topic = {
                Item: {
                    Id: '',
                    Name: '',
                }
            };
            this.description = '';
            this.selected = '';
        },

        initializeData() {
            this.singleCategoryFullWidthSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);

            if (this.singleCategoryFullWidthSettings.Name)
                this.title = this.singleCategoryFullWidthSettings.Name;
            if (this.singleCategoryFullWidthSettings.CategoryId)
                this.topicId = this.singleCategoryFullWidthSettings.CategoryId;
            if (this.singleCategoryFullWidthSettings.Description)
                this.description = this.singleCategoryFullWidthSettings.Description;
        },

        applyNewMarkdown() {

            if (this.title)
                this.singleCategoryFullWidthSettings.Name = this.title;
            this.singleCategoryFullWidthSettings.CategoryId = this.selected.Item.Id;
            if (this.description)
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

