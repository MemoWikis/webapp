declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();
declare var ripple: any;

Vue.directive("ripple", ripple.Ripple);

var FAB = Vue.component('floating-action-button',
    {
        props: ['is-topic-tab', 'create-category-url','category-id'],
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
                contentIsReady: false,
                showBar: false,
                center: true,
                width: null,
                shrink: false,
                expand: false,
                contentHasChanged: false,
                disableSave: true,
                showLoginReminder: true,
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
            contentHasChanged(val) {
                if (val) {
                    this.editMode = true;
                    this.editCategoryContent();
                }
            }
        },
        created() {
            window.addEventListener('scroll', this.handleScroll);
            window.addEventListener('resize', this.footerCheck);
            if (this.isLearningTab)
                eventBus.$on('load-questions-list', this.getEditQuestionUrl);
            eventBus.$on('tab-change', () => this.cancelEditMode());
            if (IsLoggedIn.Yes) {
                this.disableSave = false;
                this.showLoginReminder = false;
            }
        },
        mounted() {
            this.footerCheck();
            if (this.isTopicTab == "True") {
                eventBus.$on('content-is-ready',
                    () => {
                        this.contentIsReady = true;
                    });
            } else
                this.contentIsReady = true;
            eventBus.$on('content-change',
                () => {
                    this.contentHasChanged = true;
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
            openLogin() {

            },
            toggleFAB() {
                this.showMiniFAB = true;
                this.isOpen = !this.isOpen;
                if (this.editMode)
                    this.editCategoryContent();
            },
            editCategoryContent() {
                this.showFABTimer = setTimeout(() => {
                    this.showFAB = false;
                }, 300);
                this.showBar = true;
                this.isOpen = false;
            },
            createCategory() {
                if (NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg("CreateCategory");
                    return;
                }
                var self = this;
                var parent = {
                    id: self.categoryId,
                    addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                    moveCategories: false,
                    redirect: true,
                }
                if (self.isLearningTab)
                    parent.addCategoryBtnId = null;
                $('#AddCategoryModal').data('parent', parent).modal('show');
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
                this.isOpen = false;
                this.footerCheck();
            },
            footerCheck() {
                if (this.isTopicTab == "false")
                    return;

                var contentModuleAppWidth = $('#ContentModuleApp').width();
                var windowWidth = $(window).width();
                const elFooter = document.getElementById('TopicTabContentEnd');

                if (elFooter) {
                    var rect = elFooter.getBoundingClientRect();
                    var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
                    if (rect.top - viewHeight >= 0) {
                        if (this.footerIsVisible && this.editMode) {
                            this.shrink = false;
                            this.expand = true;
                        }
                        this.footerIsVisible = false;
                        this.width = windowWidth;
                    } else {
                        if (!this.footerIsVisible && this.editMode) {
                            this.expand = false;
                            this.shrink = true;
                        }
                        this.footerIsVisible = true;
                        this.width = contentModuleAppWidth;
                    }
                };
            },
            editQuestion() {
                window.location.href = this.editQuestionUrl;
            },
            getEditQuestionUrl() {
                if (IsLoggedIn.Yes)
                    this.showEditQuestionButton = true;
                else
                    this.showEditQuestionButton = false;

                var currentQuestionId = $('#AnswerBody #questionId').val();
                $.post("/Question/GetEditUrl", { id: currentQuestionId })
                    .done((result) => {
                        this.editQuestionUrl = result;
                    }).fail(() => {
                        this.editQuestionUrl = null;
                        });
            },
            saveContent() {
                if (NotLoggedIn.Yes()) {
                    NotLoggedIn.ShowErrorMsg("SaveContentWithBtn");
                    return;
                }
                eventBus.$emit('request-save');
            },
            cancelEditMode() {
                this.isOpen = false;
                this.shrink = false;
                this.expand = false;
                clearTimeout(this.showFABTimer);
                this.showFAB = true;
                this.showMiniFAB = false;

                this.editMode = false;
                this.contentHasChanged = false;
                eventBus.$emit('cancel-edit-mode');
            },
        }
    });
