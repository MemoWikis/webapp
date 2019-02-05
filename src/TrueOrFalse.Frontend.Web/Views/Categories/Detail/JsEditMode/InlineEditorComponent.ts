declare var Vue: any;
declare var Sortable: any;
var eventBus = new Vue();

Vue.component('inline-editor-component', { 
        data() {
            return {
                editor: null,
            }
        },

        methods: {
        },

    }
);

Vue.component('content-module', {

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            showMarkdownInfo: false,
        }
    },

    mounted() {
        eventBus.$on("set-edit-mode", state => this.canBeEdited = state);
    },

    methods: {

        updateHoverState(isHover) {
            const self = this;
            if (self.canBeEdited) {
                self.hoverState = isHover;
            }
        },

        deleteModule() {
            const self = this;
            self.isDeleted = true;
        },
    }
});

Vue.directive('sortable', {
    inserted(el, binding) {
        new Sortable(el, binding.value || {})
    }
});

new Vue({
    el: '#ContentModule',
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
        }
    }, 

    created() {
        eventBus.$on('save-markdown',
            (data) => {
                if (data == 'top') {
                    this.saveMarkdown(data);
                }
            });
        eventBus.$on("set-edit-mode", state => this.editMode = state);
    },
   
    methods: {

        cancelEditMode() {
            this.editMode = false;
            eventBus.$emit('set-edit-mode', this.editMode);
            this.$forceUpdate()
        },

        removeAlert() {
            this.saveMessage = '';
            this.saveSuccess = false;
            this.showTopAlert = false;
        },

        saveMarkdown(data) {
            if (data == 'top') {
                this.showTopAlert = true;
            } else {
                this.showTopAlert = false;
            };
            const markdownParts = $("li.module").map((idx, elem) => $(elem).attr("markdown")).get();
            let markdownDoc = "";
            if (markdownParts.length >= 1) 
                markdownDoc = markdownParts.reduce((list, doc) => { return list + "\r\n" + doc });

            $.post("/Category/SaveMarkdown",
                {
                    categoryId: $("#hhdCategoryId").val(), markdown: markdownDoc,
                },
                (success) => {
                    if (success == true) {
                        this.saveSuccess = true;
                        this.saveMessage = "Das Thema wurde gespeichert.";
                    } else {
                        this.saveSuccess = false;
                        this.saveMessage = "Das Speichern schlug fehl.";
                    }
                }
            )
        },
    }
});

new Vue({
    el: '#Management',
    data() {
        return {
            editMode: false
        }
    },

    created() {
        eventBus.$on("set-edit-mode", state => this.editMode = state);
    },

    methods: {
        setEditMode() {
            if (NotLoggedIn.Yes()) {
                return;
            } else {
                this.editMode = !this.editMode;
                eventBus.$emit('set-edit-mode', this.editMode);
            } 
        },

        saveMarkdown() {
            eventBus.$emit('save-markdown', 'top')
        },
    }
});