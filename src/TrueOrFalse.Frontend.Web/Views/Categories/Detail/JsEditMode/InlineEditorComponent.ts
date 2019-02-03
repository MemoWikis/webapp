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
            },
            saveSuccess: false,
            saveMessage: '',
            showSaveButton: false,
        }
    }, 

    created() {
        eventBus.$on('save',
            (data) => {
                if (data == 'markdown') {
                    this.saveMarkdown();
                }
            });
        eventBus.$on("set-edit-mode", state => this.showSaveButton = state);
    },
   
    methods: {

        removeAlert() {
            this.saveMessage = '',
            this.saveSuccess = false;
        },

        saveMarkdown() {
            const markdownParts = $("li.module").map((idx, elem) => $(elem).attr("markdown")).get();
            let markdownDoc = "";
            if (markdownParts.length >= 1) 
                markdownDoc = markdownParts.reduce((list, doc) => { return list + "\r\n" + doc });

            $.post("/Category/SaveMarkdown",
                {
                    categoryId: $("#hhdCategoryId").val(), markdown: markdownDoc,
                },
                (success) => {
//                    console.log("completed"),
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

    methods: {
        setEditMode() {
            this.editMode = !this.editMode;
            eventBus.$emit('set-edit-mode', this.editMode);
        },

        saveMarkdown() {
            eventBus.$emit('save', 'markdown')
//            eventBus.$emit('save-markdown')
//            const markdownParts = $("li.module").map((idx, elem) => $(elem).attr("markdown")).get();
//            let markdownDoc = "";
//            if (markdownParts.length >= 1)
//                markdownDoc = markdownParts.reduce((list, doc) => { return list + "\r\n" + doc });
//
//            $.post("/Category/SaveMarkdown",
//                { categoryId: $("#hhdCategoryId").val(), markdown: markdownDoc },
//                () => { console.log("completed") }
//            )
        },
    }
});