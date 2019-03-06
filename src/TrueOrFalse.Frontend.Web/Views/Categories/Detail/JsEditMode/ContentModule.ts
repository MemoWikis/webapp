﻿var contentModuleComponent = Vue.component('content-module', {

    props: {
        origMarkdown: String,
        contentModuleType: String,
    },

    data() {
        return {
            hoverState: false,
            isDeleted: false,
            canBeEdited: false,
            markdown: '',
            isListening: false,
            modalType: '',
            id: '',
            modal: '',
            button: '',
            editMe: false,
        };
    },

    created() {
        this.markdown = this.origMarkdown;
        this.modalType = '#' + this.contentModuleType + 'SettingsDialog';
        this.id = this.contentModuleType + 'Module-' + this._uid;
    },

    mounted() {
        eventBus.$on('set-edit-mode', state => this.canBeEdited = state);
        eventBus.$on('close-content-module-settings-modal', (event) => {
            if (this.isListening && event) {
                this.isDeleted = true;
                this.isListening = false;
            } else if (this.isListening && event) {
                this.isListening = false;
            };
        });
    },

    watch: {
        canBeEdited: function (val) {
            if (val) {
                this.modal = 'modal';
                this.button = 'button';
            } else {
                this.modal = '';
                this.button = '';
            };
        },
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

        getMarkdown(string) {
            console.log(string);
        },

        editInlineText() {
            if (this.canBeEdited) {
                this.editMe = true;
                this.isListening = true;
            }
        },

        moveUp() {

        },

        moveDown() {

        },
    },
});