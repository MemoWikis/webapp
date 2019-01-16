declare var Vue: any;
declare var marked: any;
declare var require: any;

Vue.component('inline-editor-component',
    { 

        data: function() {
            return {
                editOffset: -1,
                editText: {},
                editTextOri: {},
                text: [
                    { article: 'Inline Editor Test lorem ipsum etc'},
                ],
            }
            
        },

        methods: {

            startEditing(index) {
                this.editOffset = index
                this.editText = this.text[index]
                this.editTextOri = JSON.parse(JSON.stringify(this.editText))

                this.$nextTick(function () {
                    console.log('item-article-' + this.editOffset)
                    document.getElementById('item-article-' + this.editOffset).focus()
                }.bind(this))
            },
            updateText() {
                this.editOffset = -1
                this.editTextOri = {}
                this.editText = {}
            },
            cancelEditing() {
                this.$set(this.text, this.editOffset, this.editTextOri)
                this.editOffset = -1
                this.editTextOri = {}
                this.editText = {}
            },
        },

    }
);


Vue.component('content-module', {
    data: function() {
        return {
            hoverState: false,
            isDeleted: false,
        }
    },

    methods: {
        updateHoverState(isHover) {
            var self = this;
            self.hoverState = isHover;
            console.log(self.hoverState);
        },

        deleteModule() {
            var self = this;
            self.isDeleted = true;
        },
    }
});

Vue.component('content-module-edit-button', {
    data: function() {
        return {
        }
    },
    template: "<button @click='test'>test</button>"
});





new Vue({
    el: '#ContentModuleApp',
});