import { UserCreateResult } from './userCreateResult'

export class GoogleMemuchoUser {

    static async Exists(googleId: string): Promise<boolean> {

        var doesExist = await $fetch<boolean>('/apiVue/GoogleUsers/UserExists', { method: 'POST', body: { googleId: googleId }, credentials: 'include', cache: 'no-cache' 
        }).catch((error) => console.log(error.data))

        return !!doesExist;
    }

    static async Login(googleId: string, googleIdToken : string) {

        await $fetch<boolean>('/apiVue/GoogleUsers/Login', { method: 'POST', body: { googleId: googleId, googleToken: googleIdToken}, mode: 'cors', credentials: 'include', cache: 'no-cache'
        }).catch((error) => console.log(error.data))
    }
        
    static async CreateAndLogin(googleUser: gapi.auth2.GoogleUser) : Promise<boolean> {

        var basicProfile = googleUser.getBasicProfile();

        var data = { "googleUser":
            {
                GoogleId: googleUser.getId(),
                Email: basicProfile.getEmail(),
                UserName: basicProfile.getName(),
                ProfileImage: basicProfile.getImageUrl()
            }
        }

        var result = await $fetch<UserCreateResult>('/apiVue/GoogleUsers/CreateAndLogin', { method: 'POST', body: data, mode: 'cors', credentials: 'include', cache: 'no-cache'
        }).catch((error) => console.log(error.data))

        if (!!result && result.Success)
            return result.Success
        else if (!! result && !result.Success){
            //ToDo: Revoke authorization
            var reason = result.EmailAlreadyInUse == true ? " Die Email-Adresse ist bereits in Verwendung" : "";
            alert("Die Registrierung konnte nicht abgeschlossen werden." + reason);
        }
        return false;
    }
}