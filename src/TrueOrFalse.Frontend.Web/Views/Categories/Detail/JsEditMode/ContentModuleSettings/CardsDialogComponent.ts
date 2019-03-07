class CardsSettings {
    TemplateName: string = "";
    Title: string = "";
    CardOrientation: string = "";
    SetListIds: string = "";
}

Vue.component('cards-modal-component', {
    props: ['origMarkdown'],

    template: '#cards-settings-dialog-template',

    cardsSettings: CardsSettings,

    data() {
        return {
            selectedCardOrientation: '',
            title: '',
            newMarkdown: '',
            sets: [],
            newSetId: 0,
            parentId: '',
            hasPreview: false,
            vertical: false,
            settingsHasChanged: false,
            showSetInput: false,
            cardOptions: {
                animation: 100,
                fallbackOnBody: true,
                filter: 'input',
                preventOnFilter: false,
            },
        };
    },

    created() {
        var self = this;
        self.cardsSettings = new CardsSettings();
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
                this.showSetInput = false;
                this.settingsHasChanged = false;
                this.newSetId = '';
                this.newMarkdown = $('#cardsSettingsDialog').data('parent').markdown;
                this.parentId = $('#cardsSettingsDialog').data('parent').id;
                this.cardsSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
                this.selectedCardOrientation = this.cardsSettings.CardOrientation;
                this.sets = this.cardsSettings.SetListIds.split(',');
                if (this.cardsSettings.Title)
                    this.title = this.cardsSettings.Title;
            });

        $('#cardsSettingsDialog').on('hidden.bs.modal',
            function() {
                this.sets = [];
                if (!this.settingsHasChanged)
                    eventBus.$emit('close-content-module-settings-modal', false);
            });
    },

    methods: {
        hideSetInput() {
            this.newSetId = 0;
            this.showSetInput = false;
        },
    
        addCard(val) {
            this.sets.push(val);
            this.newSetId = '';
        },
        removeSet(index) {
            this.sets.splice(index, 1);
        },

        applyNewMarkdown() {
            const setIdParts = $(".cardsSettings").map((idx, elem) => $(elem).attr("setId")).get();
            if (setIdParts.length >= 1)
                this.cardsSettings.SetListIds = setIdParts.join(',');
            this.cardsSettings.Title = this.title;
            this.cardsSettings.CardOrientation = this.selectedCardOrientation;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.cardsSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            $('#cardsSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#cardsSettingsDialog').modal('hide');
        },
    },
});

