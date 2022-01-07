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

            setTimeout(() => {
                    Google.AttachClickHandler('GoogleLogin');
                },
                500);
        },

        methods: {
            FacebookLogin() {
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);
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
        new Google();
        setTimeout(() => {
                Google.AttachClickHandler('GoogleRegister');
            },
            500);

        $("#FacebookLogin").click(() => {
            FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*disallowRegistration*/ false);
        });
        $("#FacebookRegister").click(() => {
            FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*disallowRegistration*/ false);
        });
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
    },

    methods: {
        FacebookLogin() {
            FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false);
        },
    }
});