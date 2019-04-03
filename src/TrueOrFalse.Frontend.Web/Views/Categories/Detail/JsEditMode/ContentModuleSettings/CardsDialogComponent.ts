class CardsSettings {
    TemplateName: string = "";
    Title: string = "";
    CardOrientation: string = "";
    SetListIds: string = "";
};

Vue.component('cards-modal-component', {
    template: '#cards-settings-dialog-template',

    cardsSettings: CardsSettings,

    data() {
        return {
            newMarkdown: '',
            title: '',
            selectedCardOrientation: 'Landscape',
            sets: [],
            newSet: '',
            parentId: '',
            vertical: false,
            settingsHasChanged: false,
            showSetInput: false,
            cardOptions: {
                animation: 100,
                fallbackOnBody: true,
                filter: '.placeholder',
                preventOnFilter: false,
                onMove: this.onMove,
            },
            searchResults: '',
            searchType: 'Sets',
            options: [],
        };
    },

    created() {
        var self = this;
        self.cardsSettings = new CardsSettings();
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
        selectedCardOrientation: function(val) {
            if (val == 'Portrait')
                this.vertical = true;
            else this.vertical = false;
        },

        newMarkdown: function() {
            this.settingsHasChanged = true;
        },
    },
    
    mounted: function() {
        $('#cardsSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = $('#cardsSettingsDialog').data('parent').markdown;
                this.parentId = $('#cardsSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#cardsSettingsDialog').on('hidden.bs.modal',
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
            this.sets = [];
            this.settingsHasChanged = false;
            this.title = '';
            this.selectedCardOrientation = 'Landscape';
            this.newSet = '';
            this.showSetInput = false;
            this.errorMessage = '';
        },

        initializeData() {
            this.cardsSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);

            if (this.cardsSettings.Title)
                this.title = this.cardsSettings.Title;
            if (this.cardsSettings.CardOrientation)
                this.selectedCardOrientation = this.cardsSettings.CardOrientation;
            if (this.cardsSettings.SetListIds)
                this.sets = this.cardsSettings.SetListIds.split(',');
        },

        hideSetInput() {
            this.newSet = '';
            this.showSetInput = false;
        },
    
        addSet() {
            try {
                if (this.newSet.Item.Id) {
                    this.sets.push(this.newSet.Item.Id);
                    this.newSet = '';
                }
            } catch (e) { };
        },
        removeSet(index) {
            this.sets.splice(index, 1);
        },

        applyNewMarkdown() {
            if (this.sets.length > 0) {
                const setIdParts = $(".cardsDialogData").map((idx, elem) => $(elem).attr("setId")).get();
                if (setIdParts.length >= 1)
                    this.cardsSettings.SetListIds = setIdParts.join(',');
                if (this.title)
                    this.cardsSettings.Title = this.title;
                this.cardsSettings.CardOrientation = this.selectedCardOrientation;
                this.newMarkdown = Utils.ConvertJsonToMarkdown(this.cardsSettings);
                Utils.ApplyMarkdown(this.newMarkdown, this.parentId);
                $('#cardsSettingsDialog').modal('hide');
            } else {
                this.errorMessage = 'Sie müssen ein Set auswählen';
                console.log('bitte Set auswählen');
            };

        },

        closeModal() {
            $('#cardsSettingsDialog').modal('hide');
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

