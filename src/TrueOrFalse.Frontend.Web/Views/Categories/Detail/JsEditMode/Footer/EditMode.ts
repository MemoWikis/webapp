
new Vue({
    el: '#GlobalLicense',
    data() {
        return {
            editMode: false
        }
    },

    created() {
        eventBus.$on("set-edit-mode", state => this.editMode = state);
    },
});

new Vue({
    el: '#MasterFooter',
    data() {
        return {
            editMode: false
        }
    },

    created() {
        eventBus.$on("set-edit-mode", state => this.editMode = state);
    },
});