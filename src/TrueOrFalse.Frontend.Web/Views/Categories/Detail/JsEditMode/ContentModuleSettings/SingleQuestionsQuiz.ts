class SingleQuestionsQuizSettings {
    TemplateName: string = "";
    Title: string = "";
    Text: string = "";
    QuestionIds: string = "";
    Order: string = "";
    MaxQuestions: number = 5;
}

Vue.component('singlequestionsquiz-modal-component', {

    template: '#singlequestionsquiz-settings-dialog-template',

    singleQuestionsQuizSettings: SingleQuestionsQuizSettings,

    data() {
        return {
            newMarkdown: '',
            parentId: '',
            settingsHasChanged: false,
            questions: [],
            showQuestionInput: false,
            newQuestionsId: 0,
            maxQuestions: 5,
            order: '',
            title: '',
            description: '',
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
        self.singleQuestionsQuizSettings = new SingleQuestionsQuizSettings();
    },

    watch: {
        newMarkdown: function() {
            this.settingsHasChanged = true;
        },
    },
    
    mounted: function() {
        $('#singlquestionsquizSettingsDialog').on('show.bs.modal',
            event => {
                
                this.newMarkdown = $('#singlquestionsquizSettingsDialog').data('parent').markdown;
                this.parentId = $('#singlquestionsquizSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#singlquestionsquizSettingsDialog').on('hidden.bs.modal',
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
            this.newQuestionId = '';
            this.showQuestionInput = false;
        },

        initializeData() {
            this.singleQuestionsQuizSettings = Utils.ConvertEncodedHtmlToJson(this.newMarkdown);
            if (this.singleQuestionsQuizSettings.Title)
                this.title = this.singleQuestionsQuizSettings.Title;
            if (this.singleQuestionsQuizSettings.Text)
                this.description = this.singleQuestionsQuizSettings.Text;
            if (this.singleQuestionsQuizSettings.Order)
                this.order = this.singleQuestionsQuizSettings.Order;
            this.questions = this.singleQuestionsQuizSettings.QuestionsIds.split(',');
            this.maxQuestions = this.singleQuestionsQuizSettings.MaxQuestions;
        },

        hideQuestionInput() {
            this.newQuestionId = '';
            this.showQuestionInput = false;
        },
    
        addQuestion(val) {
            this.questions.push(val);
            this.newQuestionId = '';
        },
        removeQuestion(index) {
            this.questions.splice(index, 1);
        },

        applyNewMarkdown() {
            const setIdParts = $(".singleQuestionsQuizDialogData").map((idx, elem) => $(elem).attr("questionId")).get();
            if (setIdParts.length >= 1)
                this.singleQuestionsQuizSettings.SetListIds = setIdParts.join(',');
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.singleQuestionsQuizSettings);
            Utils.ApplyMarkdown(this.newMarkdown, this.parentId);
            $('#singlquestionsquizSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#singlquestionsquizSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

