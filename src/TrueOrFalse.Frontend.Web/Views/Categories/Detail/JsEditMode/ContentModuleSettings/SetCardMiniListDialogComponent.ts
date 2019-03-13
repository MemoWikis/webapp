class SetCardMiniListSettings {
    TemplateName: string = "";
    SetListIds: string = "";
}

Vue.component('setcardminilist-modal-component', {

    template: '#setcardminilist-settings-dialog-template',

    setCardMiniListSettings: SetCardMiniListSettings,

    data() {
        return {
            newMarkdown: '',
            sets: [],
            newSetId: 0,
            parentId: '',
            settingsHasChanged: false,
            showSetInput: false,
            cardOptions: {
                animation: 100,
                fallbackOnBody: true,
                filter: '.placeholder',
                preventOnFilter: false,
                onMove: this.onMove,
            },
        };
    },

    created() {
        var self = this;
        self.setCardMiniListSettings = new SetCardMiniListSettings();
    },

    watch: {
        newMarkdown: function() {
            this.settingsHasChanged = true;
        },
    },
    
    mounted: function() {
        $('#setcardminilistSettingsDialog').on('show.bs.modal',
            event => {
                
                this.newMarkdown = $('#setcardminilistSettingsDialog').data('parent').markdown;
                this.parentId = $('#setcardminilistSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#setcardminilistSettingsDialog').on('hidden.bs.modal',
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
            this.newSetId = '';
            this.showSetInput = false;
        },

        initializeData() {
            this.setCardMiniListSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
            this.sets = this.setCardMiniListSettings.SetListIds.split(',');
        },

        hideSetInput() {
            this.newSetId = '';
            this.showSetInput = false;
        },
    
        addSet(val) {
            this.sets.push(val);
            this.newSetId = '';
        },
        removeSet(index) {
            this.sets.splice(index, 1);
        },

        applyNewMarkdown() {
            const setIdParts = $(".setCardMiniListDialogData").map((idx, elem) => $(elem).attr("setId")).get();
            if (setIdParts.length >= 1)
                this.setCardMiniListSettings.SetListIds = setIdParts.join(',');
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.setCardMiniListSettings);
            Utils.UpdateMarkdown(this.newMarkdown, this.parentId);
            $('#setcardminilistSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#setcardminilistSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

