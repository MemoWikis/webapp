declare var VueTextareaAutosize: any;

Vue.use(VueTextareaAutosize);

Vue.directive('sortable',
    {
        inserted(el, binding) {
            new Sortable(el, binding.value || {});
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
                group: 'nested',
            },
            saveSuccess: false,
            saveMessage: '',
            editMode: false,
            showTopAlert: false,
            previewModule: null,
            changedMarkdown: false,
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
                    var appended = $(previewHtml).insertAfter(moduleToReplace);
                    var instance = new contentModuleComponent({
                        el: appended.get(0)
                    });
                    eventBus.$emit('close-content-module-settings-modal', event.preview);
                    eventBus.$emit('set-edit-mode', this.editMode);
                } else {
                    console.log('kein neues Markdown verfügbar');
                };
            });
    },

    mounted() {
        this.changedMarkdown = false;
    },

    methods: {
        cancelEditMode() {
            this.editMode = false;
            eventBus.$emit('set-edit-mode', this.editMode);
            if (this.changedMarkdown) {
                location.reload();
            };
        },

        removeAlert() {
            this.saveMessage = '';
            this.saveSuccess = false;
            this.showTopAlert = false;
        },

        async saveMarkdown(data) {
            if (data == 'top') {
                this.showTopAlert = true;
            } else {
                this.showTopAlert = false;
            };
            const markdownParts = $(".module").map((idx, elem) => $(elem).attr("markdown")).get();
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