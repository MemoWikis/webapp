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
                showFacebookPluginInfo: false,
                button1Text: 'Anmelden',
                showLoginIsInProgress: false,
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
        },

        watch: {
            allowGooglePlugin(val) {
                if (val)
                    this.showGooglePluginInfo = false;
            },
            allowFacebookPlugin(val) {
                if (val)
                    this.showFacebookPlugin = false;
            },
            showGooglePluginInfo(val) {
                if (val || this.showLoginIsInProgress || this.showFacebookPluginInfo)
                    this.button1Text = null;
                else
                    this.button1Text = 'Anmelden';
            },
            showFacebookPluginInfo(val) {
                if (val || this.showLoginIsInProgress || this.showGooglePluginInfo)
                    this.button1Text = null;
                else
                    this.button1Text = 'Anmelden';
            },
            showLoginIsInProgress(val) {
                if (val || this.showFacebookPluginInfo || this.showGooglePluginInfo)
                    this.button1Text = null;
                else
                    this.button1Text = 'Anmelden';
            }
        },

        methods: {
            loadGooglePlugin() {
                this.showLoginIsInProgress = true;
                this.$emit('load-google-plugin');
            },
            loadFacebookPlugin() {
                this.showLoginIsInProgress = true;
                this.$emit('load-facebook-plugin');
            },

            FacebookLogin() {
                this.showLoginIsInProgress = true;
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);
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
            },
            GoogleLogin() {
                this.showLoginIsInProgress = true;
                Google.SignIn();
            }
        }
    });


var loginApp = new Vue({
    el: '#LoginApp',
    name: 'LoginLoader',
    props: [],
    data() {
        return {
            loaded: false,
            allowGooglePlugin: false,
            allowFacebookPlugin: false,
            showGooglePluginInfo: false,
            showFacebookPluginInfo: false,
            showLoginIsInprogress: false,
        }
    },

    watch: {
        allowGooglePlugin() {
            document.cookie = "allowGooglePlugin=true";

            var googleRegister = document.getElementById("GoogleRegister");
            if (googleRegister != null)
                googleRegister.classList.remove("hidden");

            var googleRegisterPlaceholder = document.getElementById("GoogleRegisterPlaceholder");
            if (googleRegisterPlaceholder != null)
                googleRegisterPlaceholder.classList.add("hidden");
        },
        allowFacebookPlugin() {
            document.cookie = "allowFacebookPlugin=true";

            var facebookRegister = document.getElementById("FacebookRegister");
            if (facebookRegister != null)
                facebookRegister.classList.remove("hidden");

            var facebookRegisterPlaceholder = document.getElementById("FacebookRegisterPlaceholder");
            if (facebookRegisterPlaceholder != null)
                facebookRegisterPlaceholder.classList.add("hidden");
        }
    },
    created() {
        var googleCookie = document.cookie.match('(^|;)\\s*' + "allowGooglePlugin" + '\\s*=\\s*([^;]+)')?.pop() || '';
        if (googleCookie == "true")
            this.loadGooglePlugin(false);
        var facebookCookie = document.cookie.match('(^|;)\\s*' + "allowFacebookPlugin" + '\\s*=\\s*([^;]+)')?.pop() || '';
        if (facebookCookie == "true")
            this.loadFacebookPlugin(false);
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

        eventBus.$on('load-google-plugin-info',
            () => {
                if (this.allowGooglePlugin) {
                    this.showLoginIsInProgress = true;
                    Google.SignIn();
                }
                else 
                    this.loadGooglePluginInfo();
            });

        eventBus.$on('load-facebook-plugin-info',
            () => {
                if (this.allowFacebookPlugin) {
                    this.showLoginIsInProgress = true;
                    FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false);
                }
                else
                    this.loadFacebookPluginInfo();
            });

        eventBus.$on('load-google-plugin',
            () => {
                this.loadGooglePlugin(true, true);
            });

        eventBus.$on('load-facebook-plugin',
            () => {
                this.loadFacebookPlugin();
            });

        eventBus.$on('google-login',
            () => {
                this.showLoginIsInProgress = true;
                Google.SignIn();
            });

        eventBus.$on('facebook-login',
            () => {
                this.showLoginIsInProgress = true;
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false);
            });
    },

    methods: {
        loadGooglePlugin(login = true, isRegister = false) {
            this.allowGooglePlugin = true;

            var gapiScript = document.createElement('script');
            gapiScript.src = 'https://apis.google.com/js/api:client.js';
            gapiScript.onload = () => {
                var jsapi = document.createElement('script');
                jsapi.onload = () => {
                    var g = new Google();

                    setTimeout(() => {
                        if (login) {
                            this.showLoginIsInProgress = true;
                            Google.SignIn();
                        }
                    }, 500);
                };
                jsapi.src = 'https://www.google.com/jsapi';
                document.head.appendChild(jsapi);
            }
            document.head.appendChild(gapiScript);
        },

        loadFacebookPlugin(login = true) {
            this.allowFacebookPlugin = true;
            window.fbAsyncInit = function () {
                FB.init({
                    appId: '1789061994647406',
                    xfbml: true,
                    version: 'v2.8'
                });
            };

            (function (d, s, id) {
                var js, fjs = d.getElementsByTagName(s)[0];
                if (d.getElementById(id)) { return; }
                js = d.createElement(s); js.id = id;
                js.src = "//connect.facebook.net/de_DE/sdk.js";
                fjs.parentNode.insertBefore(js, fjs);
            }(document, 'script', 'facebook-jssdk'));

            if (login) {
                this.showLoginIsInProgress = true;
                setTimeout(() => {
                    FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false);
                }, 500);
            }
        },

        loadGooglePluginInfo() {
            Alerts.showInfo({
                text: '',
                customHtml: '<p>Beim Login mit Google werden Daten mit den Servern von Google ausgetauscht. Dies geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer <a href="/Impressum"> Datenschutzerklärung</a>.</p>',
                customBtn: '<div class="btn memo-button col-xs-4 btn-default" data-dismiss="modal">Abbrechen</div>' +
                    '<div class="btn memo-button col-xs-4 btn-primary" data-dismiss="modal" onclick="eventBus.$emit(\'load-google-plugin\')">Einverstanden</div>',
                title: 'Registrierung mit Google'
            });
        },

        loadFacebookPluginInfo() {
            Alerts.showInfo({
                text: '',
                customHtml: '<p>Beim Login mit Facebook werden Daten mit den Servern von Facebook ausgetauscht. Dies geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer <a href="/Impressum"> Datenschutzerklärung</a>.</p>',
                customBtn: '<div class="btn memo-button col-xs-4 btn-default" data-dismiss="modal">Abbrechen</div>' +
                    '<div class="btn memo-button col-xs-4 btn-primary" data-dismiss="modal" onclick="eventBus.$emit(\'load-facebook-plugin\')">Einverstanden</div>',
                title: 'Registrierung mit Facebook'
            });
        }
    }
});