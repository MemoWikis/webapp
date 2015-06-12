class NotLoggedIn
{
    static Yes() {

        if ($("#IsLoggedIn").val() == "False")
            return true;

        return false;
    }

    static ShowErrorMsg() {
        $('#modalNotLoggedIn').modal('show');
    }
}