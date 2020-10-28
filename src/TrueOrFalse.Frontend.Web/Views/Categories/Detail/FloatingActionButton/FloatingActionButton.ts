
var FAB = Vue.component('floating-action-button',
    {
        props: ['is-topic-tab', 'create-category-url','create-question-url'],
        data() {
            return {
                isLearningTab: this.isTopicTab == 'False',
                editMode: false,
                isOpen: false,
                showMiniFAB: false,
                footerIsVisible: false,
                showFab: true,
                timer: null,
                showEditQuestionButton: false,
                editQuestionUrl: null,
            }
        },
        watch: {
            editMode(val) {
                if (this.timer)
                    clearTimeout(this.timer);
                if (val)
                    this.timer = setTimeout(() => {
                            this.showFab = false;
                        },
                        1000);
                else 
                    this.showFab = true;
            }
        },
        created() {
            window.addEventListener('scroll', this.handleScroll);
            window.addEventListener('resize', this.footerCheck);
            if (this.isLearningTab)
                eventBus.$on('load-questions-list', this.getEditQuestionUrl);
        },
        updated() {
            this.footerCheck();
        },
        destroyed() {
            window.removeEventListener('scroll', this.handleScroll);
            window.removeEventListener('resize', this.footerCheck);
        },
        methods: {
            toggleFAB() {
                this.showMiniFAB = true;
                this.isOpen = !this.isOpen;
                if (this.editMode)
                    this.editCategoryContent();

            },
            editCategoryContent() {
                if (NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg("EditCategory");
                    return;
                }
                this.editMode = !this.editMode;
                eventBus.$emit('set-edit-mode', this.editMode);
                this.isOpen = false;

            },
            createCategory() {
                if (NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg("CreateCategory");
                    return;
                }
                window.location.href = this.createCategoryUrl;
            },
            createQuestion() {
                if (NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg("CreateQuestion");
                    return;
                }
                window.location.href = this.createQuestionUrl;
            },
            handleScroll() {
                this.scroll = true;
                this.footerCheck();
            },
            footerCheck() {
                const elFooter = document.getElementById('CategoryFooter');

                if (elFooter) {
                    var rect = elFooter.getBoundingClientRect();
                    var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
                    if (rect.top - viewHeight >= 0 || rect.bottom < 0)
                        this.footerIsVisible = false;
                    else
                        this.footerIsVisible = true;
                };
            },

            getEditQuestionUrl() {
                var currentQuestionId = $('#AnswerBody #questionId').val();
                $.post("/Question/GetEditUrl", { id: currentQuestionId })
                    .done((result) => {
                        this.editQuestionUrl = result;
                        this.showEditQuestionButton = true;
                    }).fail(() => {
                        this.editQuestionUrl = null;
                        this.showEditQuestionButton = false;
                        });
            },
            saveContent() {

            },
            cancelEditMode() {
                this.editMode = false;
                eventBus.$emit('set-edit-mode', this.editMode);
                this.isOpen = true;
            }
        }
    });