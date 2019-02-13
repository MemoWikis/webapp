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
            contentModuleJson: '',
            markdownContent: '',
        }
    },

    mounted() {
        this._cardSettings = Utils.ConvertEncodedHtmlToJson(this.markdown);

        $('#modalCardsSettings').on('show.bs.modal',
            event => {
                console.log("test");
            });
    },

    methods: {
        checkJson(json) {
            this.contentModuleJson = json;
            console.log(json);
        }
    }
});

