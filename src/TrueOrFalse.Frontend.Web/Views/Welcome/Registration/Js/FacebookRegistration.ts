class FacebookRegistration {

    constructor() {

        FB.getLoginStatus(response => {

            console.log(response);

            if (response.status === 'connected') {
                Site.RedirectToDashboard();
            }
        });

        $("#btn-login-with-facebook").click(() => {

            FB.getLoginStatus(response => {

                if (response.status === 'connected') {
                    Site.RedirectToDashboard();
                } else if (response.status === 'not_authorized') {
                    FB.login((response : any) => {

                        console.log(response);

                        var facebook_user_id = response.userId;
                        var is_new_user = false;

                        /*    


                            if(!is_registerred(facebook_user_id)){
                                is_new_user = register user();
                            }

                            login();

                            if(is_new_user)
                                redirect_to_registration_success_page();
                            else
                                redirect_to_welcome();
                        */

                    });
                }
            });
        });
    }
}

new Facebook(() => {new FacebookRegistration()});