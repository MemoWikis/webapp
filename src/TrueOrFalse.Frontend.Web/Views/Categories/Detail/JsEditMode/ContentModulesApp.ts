declare var VueTextareaAutosize: any;
declare var VueSelect: any;
declare var Sticky: any;

Vue.use(VueTextareaAutosize);
Vue.component('v-select', VueSelect.VueSelect);

var eventBus = new Vue();

Vue.directive('sortable',
    {
        inserted(el, binding) {
            new Sortable(el, binding.value || {});
        },
    });

new Vue({
    el: '#CategoryTabsApp',
    data() {
        return {};
    },

    methods: {
        sendGaEvent(val) {
            if (NotLoggedIn.Yes()) 
                Utils.SendGaEvent("NotLoggedIn", "Click", "Open" + val);
            else 
                Utils.SendGaEvent("UserAction", "Click", "Open" + val);
        }
    },
});

new Vue({
    el: '#ContentModuleApp',
    data() {
        return {
            options: {
                handle: '.Handle',
                animation: 100,
                fallbackOnBody: true,
                filter: '.placeholder',
                preventOnFilter: false,
                onMove: this.onMove,
            },
            saveSuccess: false,
            saveMessage: '',
            editMode: false,
            showTopAlert: false,
            previewModule: null,
            changedMarkdown: false,
            footerIsVisible: '',
            awaitInlineTextId: false,
        };
    },

    created() {

        eventBus.$on('save-markdown',
            (data) => {
                if (data == 'top') {
                    this.saveMarkdown(data);
                };
            });
        eventBus.$on("set-edit-mode",
            (state) => {
                this.editMode = state;
                if (this.changedMarkdown && !this.editMode) {
                    location.reload();
                }
            });
        eventBus.$on('new-markdown',
            (event) => {
                if (event.preview == true) {
                    const previewHtml = event.newHtml;
                    const moduleToReplace = event.toReplace;
                    this.changedMarkdown = true;
                    var inserted = $(previewHtml).insertAfter(moduleToReplace);
                    var instance = new contentModuleComponent({
                        el: inserted.get(0)
                    });
                    eventBus.$emit('close-content-module-settings-modal', event.preview);
                    eventBus.$emit('set-edit-mode', this.editMode);
                };
            });
        eventBus.$on('new-content-module',
            (result) => {
                if (result) {
                    if (result.position == 'before') {}
                        var inserted = $(result.newHtml).insertBefore(result.id);
                    if (result.position == 'after')
                        var inserted = $(result.newHtml).insertAfter(result.id);
                    var instance = new contentModuleComponent({
                        el: inserted.get(0)
                    });
                    this.changedMarkdown = true;
                    eventBus.$emit('set-edit-mode', this.editMode);
                    eventBus.$emit('set-new-content-module', this.editMode);
                };
            });

        window.addEventListener('scroll', this.footerCheck);
        window.addEventListener('resize', this.footerCheck);
    },

    mounted() {
        this.changedMarkdown = false;
        if ((this.$el.clientHeight + 450) < window.innerHeight)
            this.footerIsVisible = true;
    },

    updated() {
        this.footerCheck();
    },

    methods: {

        footerCheck() {
            const elLicense = document.getElementById('GlobalLicense');

            if (elLicense) {
                var rect = elLicense.getBoundingClientRect();
                var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
                if (rect.top - viewHeight >= 0 || rect.bottom < 0)
                    this.footerIsVisible = false;
                else
                    this.footerIsVisible = true;
            };
        },

        cancelEditMode() {
            if (!this.editMode)
                return;

            this.editMode = false;
            eventBus.$emit('set-edit-mode', this.editMode);
            if (this.changedMarkdown) {
                location.reload();
            };
        },

        setEditMode() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("OpenEditMode");
                return;
            } else {
                Utils.SendGaEvent("UserAction", "Click", "OpenEditMode");
                this.editMode = !this.editMode;
                eventBus.$emit('set-edit-mode', this.editMode);
            };
        },

        removeAlert() {
            this.saveMessage = '';
            this.saveSuccess = false;
            this.showTopAlert = false;
        },

        onMove(event) {
            return event.related.id !== 'ContentModulePlaceholder';;
        },

        lockModules() {
            if (!this.editMode)
                return;

            eventBus.$emit('save-text', true);
            setTimeout(this.saveMarkdown, 200);
        },

        async saveMarkdown() {

            const markdownParts = $(".ContentModule").map((idx, elem) => $(elem).attr("markdown")).get();
            let markdownDoc = "";
            if (markdownParts.length >= 1)
                markdownDoc = markdownParts.reduce((list, doc) => { return list + "\r\n" + doc });

            $.post("/Category/SaveMarkdown",
                {
                    categoryId: $("#hhdCategoryId").val(),
                    markdown: markdownDoc,
                },
                (success) => {
                    if (success == true) {
                        this.saveSuccess = true;
                        this.saveMessage = "Das Thema wurde gespeichert.";
                        location.reload();
                    } else {
                        this.saveSuccess = false;
                        this.saveMessage = "Das Speichern schlug fehl.";
                    };
                },
            );
        },
    },
});