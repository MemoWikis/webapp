declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var loginModal = Vue.component('login-modal-component',
    {
        template: '#login-modal-template',
        data() {
            return {
                eMail: '',
                password: '',
                persistentLogin: false,
                errorMessage: '',
                passwordInputType: 'password',
            }
        },
        beforeCreate() {

        },
        mounted() {
            eventBus.$on('login-clicked',
                () => {
                    var self = this;
                    self.SubmitForm();
                });
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
                    EmailAddress: self.eMail,
                    Password: self.password,
                    PersistentLogin: self.persistentLogin,
                }

                $.post("/Login/Login",
                    data,
                    (result) => {
                        if (!result.Success) {
                            self.errorMessage = result.Message;
                            return;
                        }

                        var backToLocation = Utils.GetQueryString().backTo;
                        if (backToLocation != undefined)
                            location.href = backToLocation;
                        else
                            Site.ReloadPageExceptLogoutAndRegister(result.localHref);
                    });
            }
        }
    });


var loginApp = new Vue({
    el: '#LoginApp',
    props: [],
    data() {
        return {
            loaded: false
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

        eventBus.$on('login-Facebook',
            () => {
                var self = this;
                self.FacebookLogin();
            });
        eventBus.$on('login-Google',
            () => {
                var self = this;
                self.GoogleLogin();
            });
    },

    methods: {
        FacebookLogin() {
            FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false);
        },

        GoogleLogin() {
            new Google();
            setTimeout(() => {
                    Google.AttachClickHandler('btn-login-with-google-modal');
                },
                500);
        },
    }
});