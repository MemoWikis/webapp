class GoogleMemuchoUser {

    static Exists(googleId: string): boolean {

        var doesExist = false;

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { googleId: googleId},
            url: "/Api/GoogleUsers/UserExists",
            error(error) { console.log(error); },
            success(result) { doesExist = result; }
        });

        return doesExist;
    }

    static Login(googleId: string) {

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { googleId: googleId},
            url: "/Api/GoogleUsers/Login",
            error(error) { throw error }
        });
    }
        
    static CreateAndLogin(googleUser: gapi.auth2.GoogleUser) : boolean {

        var basicProfile = googleUser.getBasicProfile();
        var success = false;

        $.ajax({
            type: 'POST', async: false, cache: false,
            data:
            {
                "googleUser":
                {
                    GoogleId: googleUser.getId(),
                    Email: basicProfile.getEmail(),
                    UserName: basicProfile.getName(),
                    ProfileImage: basicProfile.getImageUrl()
                }
            },
            url: "/Api/GoogleUsers/Login",
            success(result)
            {
                if (result.Success == "false") {

                    //ToDo: Revoke authorization

                    var reason = result.EmailAlreadyInUse == "true" ? "Die Email-Adresse ist bereits in Verwendung" : "";
                    alert("Die Registrierung konnte nicht abgeschlossen werden." + reason);

                    success = false;
                }

                success = true;                
            },
            error(error) { throw error }
        });

        return success;
    }
}