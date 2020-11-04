declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();
declare var ripple: any;

Vue.directive("ripple", ripple.Ripple);

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
                showFAB: true,
                showFABTimer: null,
                timer: null,
                showEditQuestionButton: false,
                editQuestionUrl: null,
                isExtended: true,
                fabLabel: 'Bearbeiten',
                scrollTimer: null,
                wasOpen: false,
                contentIsReady: false,
                showBar: false,
                center: true,
            }
        },
        watch: {
            isExtended(val) {
                if (val && this.isOpen)
                    this.fabLabel = 'Abbrechen';
                else if (val && !this.isOpen)
                    this.fabLabel = 'Bearbeiten';
                else if (!val)
                    this.fabLabel = '';
            },
            isOpen(val) {
                if (val && this.isExtended)
                    this.fabLabel = 'Abbrechen';
                else if (!val && this.isExtended)
                    this.fabLabel = 'Bearbeiten';
                else if (!this.isExtended)
                    this.fabLabel = '';
            },
        },
        created() {
            window.addEventListener('scroll', this.handleScroll);
            window.addEventListener('resize', this.footerCheck);
            if (this.isLearningTab)
                eventBus.$on('load-questions-list', this.getEditQuestionUrl);
        },
        mounted() {
            if ($('#ContentModuleApp').attr('openEditMode') == 'True')
                this.editMode = true;
            eventBus.$on('content-is-ready',
                () => {
                    this.contentIsReady = true;
                });
        },
        updated() {
            this.footerCheck();
        },
        destroyed() {
            window.removeEventListener('scroll', this.handleScroll);
            window.removeEventListener('resize', this.footerCheck);
            eventBus.$off('content-is-ready');
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
                this.showFABTimer = setTimeout(() => {
                    this.showFAB = false;
                }, 300);
                this.showBar = true;
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
                if (window.scrollY == 0)
                    this.isExtended = true;
                else this.isExtended = false;
                this.footerCheck();
                if (this.isOpen) {
                    this.wasOpen = true;
                    this.isOpen = false;
                }
            },
            footerCheck() {
                const elFooter = document.getElementById('TopicTabContentEnd');

                if (elFooter) {
                    var rect = elFooter.getBoundingClientRect();
                    var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
                    if (rect.top - viewHeight >= 0 || rect.bottom < 0)
                        this.footerIsVisible = false;
                    else
                        this.footerIsVisible = true;
                };
            },
            editQuestion() {
                window.location.href = this.editQuestionUrl;
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
                eventBus.$emit('request-save');
            },
            cancelEditMode() {
                clearTimeout(this.showFABTimer);
                this.showFAB = true;
                this.showMiniFAB = false;
                this.editMode = false;
                eventBus.$emit('set-edit-mode', this.editMode);
            },
        }
    });
