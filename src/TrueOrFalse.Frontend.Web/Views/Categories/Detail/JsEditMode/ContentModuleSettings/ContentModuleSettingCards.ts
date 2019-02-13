class CardSettings {
    CardOrientation: string;
    SetListIds: string;
    TemplateName: string;
}

Vue.component('modal-cards-settings', {
    props: ['markdown'],

    _cardSettings: CardSettings,

    data() {
        return {
            selectedCardOrientation: '',
        }
    },

    created() {
        this._cardSettings = Utils.ConvertEncodedHtmlToJson(this.markdown);
        this.selectedCardOrientation = this._cardSettings.CardOrientation;

    },

    methods: {
        showNewMarkdown() {
            this._cardSettings.CardOrientation = this.selectedCardOrientation;

            var encodedHtml = Utils.ConvertJsonToEncodedHtml(this._cardSettings);
            console.log('[[' + encodedHtml + ']]');
        }
    }
});

