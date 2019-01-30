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

    data() {
        return {
            hoverState: false,
            isDeleted: false,
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
    template: "<button @click='test'>test</button>"
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
                onSort: this.onSort,
                animation: 100
            }
        }
    }, 

    methods: {
        onSort(event) {
            var itemElement = event.item;
            var markdown = itemElement.getAttribute('markdown');
            console.log(
                markdown,
//                event.oldIndex,
//                event.newIndex,
            )
        }
    }

});