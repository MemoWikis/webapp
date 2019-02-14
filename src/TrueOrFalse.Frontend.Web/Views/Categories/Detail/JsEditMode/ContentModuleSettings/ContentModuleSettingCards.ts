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
        }
    },

    created() {
        console.log(this.origMarkdown);
        this._cardSettings = Utils.ConvertEncodedHtmlToJson(this.origMarkdown);
        this.newMarkdown = this.origMarkdown;
        this.selectedCardOrientation = this._cardSettings.CardOrientation;
        this.updateMarkdown();
    },

    methods: {
        showNewMarkdown() {
            this._cardSettings.CardOrientation = this.selectedCardOrientation;

            this.newMarkdown = Utils.ConvertJsonToMarkdown(this._cardSettings);
            console.log(this.newMarkdown);
            this.updateMarkdown();
        },

        updateMarkdown() {
            eventBus.$emit('set-new-markdown', this.newMarkdown);
        }
    }
});

