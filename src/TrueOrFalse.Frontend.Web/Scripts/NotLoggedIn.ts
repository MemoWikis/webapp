class NotLoggedIn
{
    static Yes() {

        if ($("#IsLoggedIn").val() == "False")
            return true;

        return false;
    }

    static ShowErrorMsg(feature = "unknown") {
        //$('#modalNotLoggedIn').modal('show');

        Login.OpenModal(null, Login.ShowFeatureInfo);

        Utils.SendGaEvent("NotLoggedIn", "Click", feature);
    }
}

class IsLoggedIn {
    
    static get Yes(): boolean {
        return !NotLoggedIn.Yes();
    }
}