var auth2: any;
var googleUser = {};

class Google {
    constructor() {

        alert("huhu");

        gapi.load('auth2', () => {
            // Retrieve the singleton for the GoogleAuth library and set up the client.
            auth2 = gapi.auth2.init(({
                client_id: '290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com',
                cookiepolicy: 'single_host_origin',
            }) as any);

            attachSignin(document.getElementById('btn-login-with-google'));

        });

        function attachSignin(element) {
            console.log(element.id);
            auth2.attachClickHandler(
                element,
                {},
                googleUser => {
                    console.log(googleUser.getBasicProfile().getName());
                },
                error => {
                    alert(JSON.stringify(error, undefined, 2));
                }
            );
        }
    }
}

$(() => {
    new Google();
});