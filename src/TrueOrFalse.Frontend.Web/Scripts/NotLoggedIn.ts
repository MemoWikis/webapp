class NotLoggedIn
{
    static Yes() {

        if ($("#IsLoggedIn").val() == "False")
            return true;

        return false;
    }

    static ShowErrorMsg(feature = "unknown") {
        $('#modalNotLoggedIn').modal('show');
        Utils.SendGaEvent("NotLoggedIn", "Click", feature);
        console.log("nli: " + feature);
    }
}