class Google {
    constructor() {

        gapi.load('auth2', () => {

            var auth2 = this.InitApi();

            var element = document.getElementById('btn-login-with-google');

            auth2.attachClickHandler(element, {},
                googleUser => this.OnLoginSuccess(googleUser),
                error => this.OnLoginError(error)
            );
        });
    }

    InitApi() {
        return gapi.auth2.init(({
            client_id: '290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com',
            cookiepolicy: 'single_host_origin',
        }) as any);
    }

    OnLoginSuccess(googleUser : gapi.auth2.GoogleUser) {

        var googleId = googleUser.getBasicProfile().getName();

        if (GoogleMemuchoUser.Exists(googleId)) {
            GoogleMemuchoUser.Login(googleId);
            Site.ReloadPage();
            return;
        }

        if (GoogleMemuchoUser.CreateAndLogin(googleUser)) {
            Site.RedirectToRegistrationSuccess();
        }
    }

    OnLoginError(error) {
        alert(JSON.stringify(error, undefined, 2));
    }

    UserExists(userId) {
        return false;
    }
}

$(() => {
    new Google();
});