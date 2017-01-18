class GoogleMemuchoUser {

    static Exists(googleId: string): boolean {

        var doesExist = false;

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { googleId: googleId},
            url: "/Api/Users/GoogleUserExists",
            error(error) { console.log(error); },
            success(result) { doesExist = result; }
        });

        return doesExist;
    }

    static Login(googleId: string) {

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { facebookUserId: googleId},
            url: "/Api/Users/Login/",
            error(error) { throw error }
        });
    }
}