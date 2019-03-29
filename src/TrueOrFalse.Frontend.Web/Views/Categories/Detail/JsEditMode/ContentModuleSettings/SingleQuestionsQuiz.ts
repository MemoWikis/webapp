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
            newQuestionId: 0,
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
        $('#singlequestionsquizSettingsDialog').on('show.bs.modal',
            event => {
                
                this.newMarkdown = $('#singlequestionsquizSettingsDialog').data('parent').markdown;
                this.parentId = $('#singlequestionsquizSettingsDialog').data('parent').id;
                this.initializeData();
            });

        $('#singlequestionsquizSettingsDialog').on('hidden.bs.modal',
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
            this.questions = [];
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
            if (this.singleQuestionsQuizSettings.QuestionIds)
                this.questions = this.singleQuestionsQuizSettings.QuestionIds.split(',');
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
            const questionIdParts = $(".singleQuestionsQuizDialogData").map((idx, elem) => $(elem).attr("questionId")).get();
            if (questionIdParts.length >= 1)
                this.singleQuestionsQuizSettings.QuestionIds = questionIdParts.join(',');
            this.newMarkdown = Utils.ConvertJsonToMarkdown(this.singleQuestionsQuizSettings);
            Utils.ApplyMarkdown(this.newMarkdown, this.parentId);
            $('#singlequestionsquizSettingsDialog').modal('hide');
        },

        closeModal() {
            $('#singlequestionsquizSettingsDialog').modal('hide');
        },

        onMove(event) {
            return event.related.id !== 'addCardPlaceholder';;
        },
    },
});

