declare var Vue: any;
declare var marked: any;

import VueNestable from 'vue-nestable';

export default {
    data() {
        return {
            nestableItems: [
                {
                    key: 0,
                    class: 'purple-text-color',
                    text: 'Andy'
                }, {
                    key: 1,
                    class: 'blue-text-color',
                    text: 'Harry',
                    nested: [
                        {
                            key: 2,
                            text: 'David'
                        }
                    ]
                }, {
                    key: 3,
                    class: 'red-text-color',
                    text: 'Lisa'
                }
            ]
        }
    }
};

Vue.use(VueNestable);

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
                hoverState: false,

                border: {
                    borderWidth: '0px',
                    borderWidthHover: "1px"
                },
            }
            
        },

        computed: {
            styling() {
                var modifier = '';
                if (this.hoverState)
                    modifier = 'Hover';

                return {
                    borderWidth: this.border['borderWidth' + modifier],
                };
            },
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

            updateHoverState(isHover) {
                this.hoverState = isHover;
            },
        },

    }
);


Vue.component('content-module', {
    data: function () {
        return {
            count: 0
        }
    }
});

Vue.component('content-module-edit-button', {
    data: function() {
        return {
            count: 0
        }
    },
    template: "<button @click='test'>test</button>"
});





new Vue({
    el: '#TopicTabContent'
});