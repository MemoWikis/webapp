class ContentModuleTemplate {
    TemplateName: string = "";
};

Vue.component('content-module-selection-modal-component', {
    template: '#content-module-selection-dialog-template',

    contentModuleTemplate: ContentModuleTemplate,

    data() {
        return {
            templateMarkdown: '',
            contentModules: [
                { type: 'Cards', name: 'Lernset', tooltip: 'Zeige Lernsets in Kartenform an.' },
                { type: 'CategoryNetwork', name: 'Themennetzwerk', tooltip: 'Über- und untergeordnete Themen werden übersichtlich dargestellt.' },
                { type: 'ContentLists', name: 'Inhaltsverzeichnis', tooltip: 'Alle Lernsets und Fragen eines Themas und der untergeordneten Themen sind in Listenform dargestellt.' },
                { type: 'EducationOfferList', name: 'Aus- und Weiterbildungen', tooltip: 'Zeigt Aus- und Weiterbildung (Universitäten, Kurse, Professoren/Dozenten etc.) eines Themas an.' },
                { type: 'InlineText', name: 'Freier Inhalt', tooltip: 'Freie Text Gestaltung per Markdown.' },
                { type: 'MediaList', name: 'Medien', tooltip: 'Zeigt Medien (Bücher, Zeitungsartikel, Online-Beiträge, Videos etc.) eines Themas an.' },
                { type: 'SetCardMiniList', name: 'Lernsetliste', tooltip: 'Zeigt eine Liste aller lernsets zu einem Thema.' },
                { type: 'SingleCategoryFullWidth', name: 'Einzelthema', tooltip: 'Zeigt ein Thema in voller Breite an.' },
                { type: 'SingleQuestionsQuiz', name: 'Quiz Widget', tooltip: 'Fragen eines Themas sind einzeln untereinander aufgelistet.' },
                { type: 'SingleSetFullWidth', name: 'Einzellernset', tooltip: 'Zeigt ein Lernset in voller Breite an.' },
                { type: 'Spacer', name: 'Abstandhalter', tooltip: 'Sorgt für mehr ßraum unter oder über einem Modul' },
                { type: 'TopicNavigation', name: 'Themenliste', tooltip: 'Zeigt eine Liste aller Hauptthemen.' },
                { type: 'VideoWidget', name: 'Video Widget', tooltip: 'Zeigt ein Video aus einem Lernset mit entsprechenden Fragen.' }
            ],
            selectedModule: '',
            modalType: '',
            modulePosition: '',
            moduleId: '',
        };
    },

    created() {
        var self = this;
        self.contentModuleTemplate = new ContentModuleTemplate();
    },

    watch: {
        selectedModule: function(val) {
            if (val != 'InlineText' &&
                val != 'Spacer' &&
                val != 'MediaList' &&
                val != 'ContentList' &&
                val != 'EducationOfferList')
                this.modalType = '#' + this.selectedModule.toLowerCase() + 'SettingsDialog';
            else
                this.modalType = false;
        }
    },

    mounted: function() {
        $('#ContentModuleSelectionModal').on('show.bs.modal',
            event => {
                this.modulePosition = $('#ContentModuleSelectionModal').data('data').position;
                this.moduleId = $('#ContentModuleSelectionModal').data('data').id;
            });

        $('#ContentModuleSelectionModal').on('hidden.bs.modal',
            event => {
                if (!this.settingsHasChanged)
                    eventBus.$emit('close-content-module-settings-modal', false);
                this.clearData();
            });
    },

    methods: {
        setActive(val) {
            this.selectedModule = val;
        },

        clearData() {
            this.templateMarkdown = '';
            this.selectedModule = '';
            this.modalType = '';
        },

        selectModule() {
            this.contentModuleTemplate.TemplateName = this.selectedModule;
            this.templateMarkdown = Utils.ConvertJsonToMarkdown(this.contentModuleTemplate);
            let template = {
                id: this.modulePosition + ':' + this.moduleId,
                markdown: this.templateMarkdown,
            };
            $('#ContentModuleSelectionModal').modal('hide');

            if (this.modalType)
                $(this.modalType).data('parent', template).modal('show');
            else if (this.selectedModule == 'InlineText')
                Utils.ApplyMarkdown('', template.id);
            else
                Utils.ApplyMarkdown(template.markdown, template.id);
        },

        closeModal() {
            $('#ContentModuleSelectionModal').modal('hide');
        },
    },
});

