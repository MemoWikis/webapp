declare var Vue: any;
declare var Sortable: any;
declare var Editor: any;

Vue.component('inline-editor-component', { 
        data() {
            return {
                editor: null,
            }
        },

        methods: {

            startEditing(index) {
                this.editOffset = index;
                this.editText = this.text[index];
                this.editTextOri = JSON.parse(JSON.stringify(this.editText));

                this.$nextTick(function() {
                    console.log('item-article-' + this.editOffset);
                    document.getElementById('item-article-' + this.editOffset).focus();
                }.bind(this));
            },
            updateText() {
                this.editOffset = -1;
                this.editTextOri = {}
                this.editText = {}
            },
            cancelEditing() {
                this.$set(this.text, this.editOffset, this.editTextOri);
                this.editOffset = -1;
                this.editTextOri = {}
                this.editText = {}
            },
        },

    }
);

Vue.component('content-module', {

    props: ['editState'],

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeSorted: false,
        }
    },

    watch: {
        editState: function (newValue, oldValue) {
            this.canBeSorted = newValue;
            console.log(newValue + oldValue);
        }
    },

    methods: {

        updateHoverState(isHover) {
            const self = this;
            self.hoverState = isHover;
        },

        deleteModule() {
            const self = this;
            self.isDeleted = true;
        },
    }
});

Vue.component('content-module-edit-button', {
    data() {
        return {
        }
    },
});

Vue.directive('sortable', {
    inserted(el, binding) {
        new Sortable(el, binding.value || {})
    }
});

new Vue({
    el: '#module',
    data() {
        return {
            options: {
                handle: '.Handle',
                animation: 100,
            },
            showSaveButton: false,
            editState: false
        }
    }, 

    mounted() {      
    },

    methods: {
        setEditMode() {
            this.editState = !this.editState;
            this.showSaveButton = !this.showSaveButton;
            console.log('editState is ' + this.editState)
        },

        save() {
            const markdownParts = $("li.module").map((idx, elem) => $(elem).attr("markdown")).get();
            const markdownDoc = markdownParts.reduce((a, b) => { return a + "\r\n" + b });

            $.post("/Category/SaveMarkdown",
                { categoryId: $("#hhdCategoryId").val(), markdown: markdownDoc },
                () => { console.log("completed") }
            );
        },
    }
});


Vue.component('category-management-buttons', {
    data() {
        return {
            editMode: false
        }
    },

    methods: {
        setEditMode() {
            this.editMode = !this.editMode;
            console.log(this.editMode)
        }
    }
});

new Vue({
    el: '#Management'
});