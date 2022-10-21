declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var loginModal = Vue.component('login-modal-component',
    {
        props: ['allowGooglePlugin', 'allowFacebookPlugin'],
        template: '#login-modal-template',
        data() {
            return {
                eMail: '',
                password: '',
                persistentLogin: false,
                errorMessage: '',
                passwordInputType: 'password',
                isWiki: false,
                tryLogin: false,
                showGooglePluginInfo: false,
            }
        },
        beforeCreate() {

        },
        mounted() {
            this.isWiki = $('#hddIsWiki').val() == 'True';
            eventBus.$on('login-clicked',
                () => {
                    var self = this;
                    self.SubmitForm();
                });
            if (this.allowGooglePlugin) {
                setTimeout(() => {
                        Google.AttachClickHandler('GoogleLogin');
                    },
                    500);
            }
        },

        methods: {

            loadGooglePlugin() {
                this.$emit('load-google-plugin');
            },

            FacebookLogin() {
                if (this.allowFacebookPlugin) {
                    FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);
                }
            },

            SubmitForm() {
                if (this.tryLogin)
                    return;
                this.tryLogin = true;

                var self = this;
                var data = {
                    EmailAddress: self.eMail,
                    Password: self.password,
                    PersistentLogin: self.persistentLogin,
                }

                $.post("/Login/Login",
                    data,
                    (result) => {
                        self.tryLogin = false;
                        if (!result.Success) {
                            self.errorMessage = result.Message;
                            return;
                        }

                        var backToLocation = Utils.GetQueryString().backTo;
                        if (backToLocation != undefined && !this.isWiki)
                            location.href = backToLocation;
                        else if (this.isWiki)
                            location.href = "/";
                        else
                            Site.LoadValidPage(result.localHref);
                    });
            }
        }
    });


var loginApp = new Vue({
    el: '#LoginApp',
    props: [],
    data() {
        return {
            loaded: false,
            allowGooglePlugin: false,
            allowFacebookPlugin: false,
        }
    },
    created() {
        var cookie = document.cookie.match('(^|;)\\s*' + "allowGooglePlugin" + '\\s*=\\s*([^;]+)')?.pop() || '';
        if (cookie == "true") {
            this.loadGooglePlugin();
        }
    },
    mounted() {

        $("#FacebookLogin").click(() => {
            this.FacebookLogin();
        });
        $("#FacebookRegister").click(() => {
            this.FacebookLogin();
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
        loadGooglePlugin() {
            var gapiScript = document.createElement('script');
            gapiScript.src = 'https://apis.google.com/js/api:client.js';
            document.head.appendChild(gapiScript);

            var jsapi = document.createElement('script');
            jsapi.onload = function () {
                var g = new Google();

                setTimeout(() => {
                        Google.AttachClickHandler('GoogleRegister');
                        Google.AttachClickHandler('GoogleLogin');
                    },
                    500);
            };
            jsapi.src = 'https://www.google.com/jsapi';
            document.head.appendChild(jsapi);

            this.allowGooglePlugin = true;
        },

        FacebookLogin() {
            if (this.allowFacebookPlugin) {
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false);
            }
        },
    }
});