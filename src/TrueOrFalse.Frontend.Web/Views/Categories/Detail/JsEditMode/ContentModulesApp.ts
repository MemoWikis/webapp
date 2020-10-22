declare var Vue: any;
declare var VueTextareaAutosize: any;
declare var VueSelect: any;
declare var Sticky: any;
declare var Sortable: any;
declare var tiptapBuild: any;
declare var hljsBuild: any;

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
                onUpdate: () => {
                    this.updateModuleOrder();
                    this.changedContent = true;
                },
                axis: 'y'
            },
            saveSuccess: false,
            saveMessage: '',
            editMode: false,
            showTopAlert: false,
            previewModule: null,
            changedContent: false,
            footerIsVisible: '',
            awaitInlineTextId: false,
            moduleOrder: [],
            modules: [],
            sortedModules: [],
            fabIsOpen: false,
        };
    },

    created() {
        eventBus.$on('get-module',
            (module) => {
                var index = this.modules.findIndex((m) => m.id == module.id);
                if (index >= 0)
                    this.modules[index] = module;
                else
                    this.modules.push(module);
            });
        eventBus.$on("set-edit-mode",
            (state) => {
                this.editMode = state;
                if (this.changedContent && !this.editMode) {
                    location.reload();
                }
            });
        eventBus.$on('update-content-module',
            (event) => {
                if (event.preview == true) {
                    const previewHtml = event.newHtml;
                    const moduleToReplace = event.toReplace;
                    var inserted = $(previewHtml).insertAfter(moduleToReplace);
                    new contentModuleComponent({
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
                    eventBus.$emit('set-edit-mode', this.editMode);
                    eventBus.$emit('set-new-content-module', this.editMode);
                    this.updateModuleOrder();
                    this.sortModules();
                };
            });

        window.addEventListener('scroll', this.footerCheck);
        window.addEventListener('resize', this.footerCheck);
        eventBus.$on('content-change',
            () => {
                if (this.editMode)
                        this.changedContent = true;
            });
    },

    mounted() {
        this.changedContent = false;
        if ((this.$el.clientHeight + 450) < window.innerHeight)
            this.footerIsVisible = true;
        if (this.$el.attributes.openEditMode.value == 'True')
            this.setEditMode();
    },

    updated() {
        this.footerCheck();
    },

    watch: {
        editMode(val) {
            if (val) {
                this.sortModules();
            }
        },
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
            if (!this.editMode)
                return;

            this.editMode = false;
            eventBus.$emit('set-edit-mode', this.editMode);
            if (this.changedContent)
                location.reload();
        },

        setEditMode() {
            this.modules = [];

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

            sorting.forEach(function(key) {
                var found = false;
                items = items.filter(function(item) {
                    if (!found && item.id == key) {
                        result.push(item.contentData);
                        found = true;
                        return false;
                    } else
                        return true;
                });
            });

            this.sortedModules = result;
            if ((result.length == 0 || result[result.length - 1].TemplateName != 'InlineText') && (items.length == 0 || items[items.length - 1].contentData.TemplateName != 'InlineText'))
                eventBus.$emit('add-inline-text-module');
            return;
        },

        removeAlert() {
            this.saveMessage = '';
            this.saveSuccess = false;
            this.showTopAlert = false;
        },

        onMove(event) {
            return event.related.id !== 'ContentModulePlaceholder';;
        },

        async saveContent() {
            if (!this.editMode)
                return;

            await this.sortModules();
            var filteredModules = null;
            if (this.sortedModules.length > 0)
                filteredModules = this.sortedModules.filter(o => (o.TemplateName != 'InlineText' || o.Content));
            var data = {
                categoryId: $("#hhdCategoryId").val(),
                content: filteredModules,
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
                        if (window.location.href.endsWith('?openEditMode=True'))
                            location.href = window.location.href.slice(0, -18);
                        else location.reload();
                    } else {
                        this.saveSuccess = false;
                        this.saveMessage = "Das Speichern schlug fehl.";
                    };
                },
            });
        }
    },
});