class CardSettings {
    Title: string;
    CardOrientation: string;
    SetListIds: string;
}

Vue.component('modal-cards-settings', {
    props: ['origMarkdown'],

    _cardSettings: CardSettings,

    data() {
        return {
            selectedCardOrientation: '',
            newMarkdown: '',
            sets: [],
            newSetId: 0,
        }
    },

    created() {
        this._cardSettings = Utils.ConvertEncodedHtmlToJson(this.origMarkdown);
        this.newMarkdown = this.origMarkdown;
        this.selectedCardOrientation = this._cardSettings.CardOrientation;
        this.updateMarkdown();
        this.sets = this._cardSettings.SetListIds.split(',');
    },

    methods: {
        addSet(val) {
            this.sets.push(val);
        },
        removeSet(index) {
            this.sets.splice(index, 1);
        },

        applyNewMarkdown() {
            const setIdParts = $("li.cardSettings").map((idx, elem) => $(elem).attr("setId")).get();
            if (setIdParts.length >= 1)
                this._cardSettings.SetListIds = setIdParts.join(',');

            this._cardSettings.CardOrientation = this.selectedCardOrientation;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this._cardSettings);
            this.updateMarkdown();
        },

        updateMarkdown() {
            eventBus.$emit('set-new-markdown', this.newMarkdown);
        }
    }
});

