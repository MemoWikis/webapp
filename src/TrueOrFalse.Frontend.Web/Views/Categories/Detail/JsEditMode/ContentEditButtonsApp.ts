var eventBus = new Vue();

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
//                if (!this.editMode) {
//                    location.reload();
//                };
            };
        },
         
        saveMarkdown() {
            eventBus.$emit('save-markdown', 'top');
        },
    }
});