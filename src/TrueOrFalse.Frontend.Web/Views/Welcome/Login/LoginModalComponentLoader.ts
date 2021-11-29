if (eventBus == null)
    var eventBus = new Vue();

Vue.component('login-modal-component-loader',
    {
        template: '#login-modal-component-loader',
        props: ['showLoginModalProp'],
        data() {
            return {
                showLoginModal: false,
                eMail: 'Hi',
                password: String,
                persistentLogin: false,
                errorMessage: String
            }
        },
        beforeCreate() {



        },
        mounted() {

        },

        methods: {
            FacebookLogin() {
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);
            },

            GoogleLogin() {
                new Google();
                setTimeout(() => {
                        Google.AttachClickHandler('btn-login-with-google-modal');
                    },
                    500);
            },

            SubmitForm() {

                var self = this;

                var data = {
                    EmailAddress: self.email ,
                    Password: self.password,
                    PersistentLogin: self.persistentLogin
                }

                $.post("/Login/Login", data, (result) => {
                    if (!result.Success) {
                        self.errorMessage = result.Message;
                        return;
                    }

                    var backToLocation = Utils.GetQueryString().backTo;
                    if (backToLocation != undefined)
                        location.href = backToLocation;
                    else
                        Site.ReloadPage_butNotTo_Logout(result.localHref);
                });
            }
        }
    });


var loginApp = new Vue({
    el: '#LoginApp',
    props: [],
    data() {
        return {
            loaded: true
    }
    },
    mounted() {
        eventBus.$on('show-login-modal',
            () => {
                this.loaded = true;
            });
        eventBus.$on('close-modal',
            () => {
                this.loaded = false;
            });
    },
});
