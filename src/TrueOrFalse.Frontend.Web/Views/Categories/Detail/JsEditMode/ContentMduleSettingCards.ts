Vue.component('content-module-settings', {

    data() {
        return {
            contentModuleJson: '',
            markdownContent: '',
        }
    },

    mounted() {
        $('#modalContentModuleSettings').on('show.bs.modal',
            event => {
            });
    },

    methods: {
        checkJson(json) {
            this.contentModuleJson = json;
            console.log(json);
        }
    }
});

Vue.directive('sortable', {
    inserted(el, binding) {
        new Sortable(el, binding.value || {})
    }
});