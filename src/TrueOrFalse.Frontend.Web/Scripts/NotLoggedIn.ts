class NotLoggedIn
{
    static Yes() {

        if ($("#IsLoggedIn").val() == "False")
            return true;

        return false;
    }

    static ShowErrorMsg(feature = "unknown") {
        Site.CloseAllModals();
        eventBus.$emit('show-login-modal');

        //$('#modalNotLoggedIn').modal('show');
        //Login.OpenModal(null, Login.ShowFeatureInfo);
    }
}

class IsLoggedIn {
    
    static get Yes(): boolean {
        return !NotLoggedIn.Yes();
    }
}