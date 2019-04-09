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
            newQuestion: 0,
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
            searchResults: '',
            searchType: 'Questions',
            options: [],
        };
    },

    created() {
        var self = this;
        self.singleQuestionsQuizSettings = new SingleQuestionsQuizSettings();
    },

    computed: {
        filteredSearch() {
            let results = [];

            if (this.searchResults)
                results = this.searchResults.Items.filter(i => i.Type === this.searchType);

            return results;
        },
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
            this.newQuestion = '';
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
            this.newQuestion = '';
            this.showQuestionInput = false;
        },
    
        addQuestion() {
            try {
                if (this.newQuestion.Item.Id) {
                    this.questions.push(this.newQuestion.Item.Id);
                    this.newQuestion = '';
                }
            } catch (e) { };
        },
        removeQuestion(index) {
            this.questions.splice(index, 1);
        },

        applyNewMarkdown() {
            const questionIdParts = $(".singleQuestionsQuizDialogData").map((idx, elem) => $(elem).attr("questionId")).get();
            if (questionIdParts.length >= 1)
                this.singleQuestionsQuizSettings.QuestionIds = questionIdParts.join(',');
            if (this.title)
                this.singleQuestionsQuizSettings.Title = this.title;
            if (this.Text)
                this.singleQuestionsQuizSettings.Text = this.description;
            if (this.Order)
                this.singleQuestionsQuizSettings.Order = this.order;
            if (this.maxQuestions)
                this.singleQuestionsQuizSettings.MaxQuestions = this.maxQuestions;
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

        onSearch(search, loading) {
            loading(true);
            this.search(loading, search, this);
        },
        search: _.debounce(function (loading, search, vm) {
            $.get("/Api/Search/ByName?term=" + search + "&type=" + this.searchType,
                (result) => {
                    this.searchResults = result;
                    vm.options = this.filteredSearch;
                    loading(false);
                }
            );
        }, 350),
    },
});

