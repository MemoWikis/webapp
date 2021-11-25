Vue.component('login-modal-component-loader',
    {
        template: '#login-modal-component-loader',
        props: [],
        data() {
            return {
                showLoginModal: false
        }
        },
        beforeCreate() {
            eventBus.$on('close-modal', () => {
                eventBus.$emit('unload-comment');
            });
            eventBus.$on('show-login-modal', () => {
                console.log("OpenLoginModal");
                Login.OpenModal();
            });

        },
        mounted() {

        }
    });