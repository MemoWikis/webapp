class CardSettings {
    Title: string = "";
    CardOrientation: string = "";
    SetListIds: string = "";
}

Vue.component('cards-modal-settings', {
    props: ['origMarkdown'],

    template: '#cards-settings-dialog-template',

     _cardSettings: CardSettings,

    data() {
        return {
            selectedCardOrientation: '',
            newMarkdown: '',
            sets: [],
            newSetId: 0,
            parentId: '',
            result: '',
            preview: '',
        }
    },

    created() {
        var self = this;
        self._cardSettings = new CardSettings();
    },

    mounted: function () {
        $('#cardsSettingsDialog').on('show.bs.modal',
            event => {
                this.newMarkdown = event.relatedTarget.getAttribute('data-markdown');
                this.parentId = event.relatedTarget.getAttribute('data-component-id');
                this._cardSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
                this.selectedCardOrientation = this._cardSettings.CardOrientation;
                this.sets = this._cardSettings.SetListIds.split(',');
            });

        $('#cardsSettingsDialog').on('hidden.bs.modal', function () {
            this.sets = [];
            this.preview = false;
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
            const setIdParts = $("li.cardSettings").map((idx, elem) => $(elem).attr("setId")).get();
            if (setIdParts.length >= 1)
                this._cardSettings.SetListIds = setIdParts.join(',');

            this._cardSettings.CardOrientation = this.selectedCardOrientation;
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this._cardSettings);
            this.updateMarkdown();
        },
        
        updateMarkdown() {
            $.post("/Category/RenderMarkdown/", { categoryId: $("#hhdCategoryId").val(), markdown: this.newMarkdown },
                (result) => {
                    this.preview = true;
                    eventBus.$emit('new-markdown', { preview: this.preview, newHtml: result, toReplace: 'li#' + this.parentId });
                    $('#cardsSettingsDialog').modal('hide');
                }
            );
        }
    }
});

