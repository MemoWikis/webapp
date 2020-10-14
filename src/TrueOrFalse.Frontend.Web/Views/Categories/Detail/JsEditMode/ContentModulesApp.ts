declare var Vue: any;
declare var VueTextareaAutosize: any;
declare var VueSelect: any;
declare var Sticky: any;
declare var Sortable: any;
declare var tiptapBuild: any;


Vue.component('v-select', VueSelect.VueSelect);



declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.directive('sortable',
    {
        inserted(el, binding) {
            new Sortable(el, binding.value, 
                {
                    onUpdate: function() {
                        eventBus.$emit('sortable-update');
                    }
                });
        },
    });

new Vue({
    props: ['content'],
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
                onUpdate: this.updateModuleOrder,
                axis: 'y'
    },
            saveSuccess: false,
            saveMessage: '',
            editMode: false,
            showTopAlert: false,
            previewModule: null,
            changedMarkdown: false,
            footerIsVisible: '',
            awaitInlineTextId: false,
            moduleOrder: [],
            modules: [],
            sortedModules: [],
        };
    },

    created() {
        eventBus.$on('get-module',
            (module) => {
                this.modules.push(module);
                this.updateModuleOrder();
            });
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

        updateModuleOrder() {
            this.moduleOrder = $(".inlinetext, .topicnavigation").map((idx, elem) => $(elem).attr("uid")).get();
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

        cancelEditMode() {

            this.sortModules();
            this.modules = [];

            if (!this.editMode)
                return;

            this.editMode = false;
            eventBus.$emit('set-edit-mode', this.editMode);
            if (this.changedMarkdown) {
                location.reload();
            };
        },

        setEditMode() {
            this.modules = [];
            this.sortModules();

            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("OpenEditMode");
                return;
            } else {
                this.editMode = !this.editMode;
                eventBus.$emit('set-edit-mode', this.editMode);
            };
        },

        sortModules() {
            var items = this.modules;
            var sorting = this.moduleOrder;
            var result = [];

            console.log(items);
            console.log(sorting);


            sorting.forEach(function(key) {
                var found = false;
                items = items.filter(function(item) {
                    if (!found && item[0] == key) {
                        result.push(item[1]);
                        found = true;
                        return false;
                    } else
                        return true;
                });
            });

            console.log(result);

            this.sortedModules = result;
        },

        removeAlert() {
            this.saveMessage = '';
            this.saveSuccess = false;
            this.showTopAlert = false;
        },

        onMove(event) {
            return event.related.id !== 'ContentModulePlaceholder';;
        },

        saveMarkdown() {

            if (!this.editMode)
                return;

            const markdownParts = $(".inlinetext, .topicnavigation").map((idx, elem) => $(elem).attr("markdown")).get();
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

        saveContent() {
            if (!this.editMode)
                return;

            this.sortModules();
            if (this.sortedModules.length == 0)
                return;
            //var jsonString = JSON.stringify(this.sortedModules);


            //$.post("/Category/SaveCategoryContent",
            //    {
            //        categoryId: $("#hhdCategoryId").val(),
            //        content: safeString,
            //    },
            //    (success) => {
            //        if (success == true) {
            //            this.saveSuccess = true;
            //            this.saveMessage = "Das Thema wurde gespeichert.";
            //            location.reload();
            //        } else {
            //            this.saveSuccess = false;
            //            this.saveMessage = "Das Speichern schlug fehl.";
            //        };
            //    },
            //);
            var data = {
                categoryId: $("#hhdCategoryId").val(),
                content: this.sortedModules,
            }

            $.ajax({
                type: 'post',
                contentType: "application/json",
                url: '/Category/SaveCategoryContent',
                data: JSON.stringify(data),
                success: function (success) {
                    if (success == true) {
                        this.saveSuccess = true;
                        this.saveMessage = "Das Thema wurde gespeichert.";
                        location.reload();
                    } else {
                        this.saveSuccess = false;
                        this.saveMessage = "Das Speichern schlug fehl.";
                    };
                },
            });
        }
    },
});



//new Vue({
//    el: "#EditorApp",
//    data: {
//        tiptap: Object.keys(tiptap),
//        tiptapUtils: Object.keys(tiptapUtils),
//        tiptapCommands: Object.keys(tiptapCommands),
//        tiptapExtensions: Object.keys(tiptapExtensions),
//        editor: new tiptap.Editor({
//            content: '<p>This is just a boring paragraph</p>',
//        }),
//    }
//});