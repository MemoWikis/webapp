class CardsSettings {
    TemplateName: string = "";
    CardOrientation: string = "";
    SetListIds: string = "";
}

Vue.component('cards-modal-component', {
    props: ['origMarkdown'],

    template: '#cards-settings-dialog-template',

     _cardsSettings: CardsSettings,

    data() {
        return {
            selectedCardOrientation: '',
            newMarkdown: '',
            sets: [],
            newSetId: 0,
            parentId: '',
        };
    },

    created() {
        var self = this;
        self._cardsSettings = new CardsSettings();
    },

    mounted: function () {
        $('#cardsSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = event.relatedTarget.getAttribute('markdown');
                this.parentId = event.relatedTarget.getAttribute('id');
                this._cardsSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
                this.selectedCardOrientation = this._cardsSettings.CardOrientation;
                this.sets = this._cardsSettings.SetListIds.split(',');
            });

        $('#cardsSettingsDialog').on('hidden.bs.modal', function () {
            this.sets = [];
        });
    },

    methods: {
        addSet(val) {
            this.sets.push(val);
        },
        removeSet(index) {
            this.sets.splice(index, 1);
        },

        applyNewMarkdown() {
            const setIdParts = $("li.cardsSettings").map((idx, elem) => $(elem).attr("setId")).get();
            if (setIdParts.length >= 1)
                this._cardsSettings.SetListIds = setIdParts.join(',');
            this._cardsSettings.CardOrientation = this.selectedCardOrientation;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this._cardsSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            $('#cardsSettingsDialog').modal('hide');
        },
    },
});

