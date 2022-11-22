export class Facebook {

    static GetUser(facebookId: string, accessToken: string, continuation: (user: FacebookUserFields)=> void){
        FB.api(
            "/" + facebookId,
            { access_token: accessToken, fields: 'name, email' },
            response => {
                if (response && !response.error) {
                    continuation(response);
                } else {    
                    throw (response);
                }
            });
    }

    static RevokeUserAuthorization(facebookId: string, accessToken: string) {
        FB.api("/me/permissions", "DELETE", response => {
            console.log(response); //return true on "app delete" success 
        });
    }
}

export interface FacebookUserFields {
    id;
    email;
    name;
}