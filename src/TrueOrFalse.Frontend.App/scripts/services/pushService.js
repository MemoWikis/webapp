app.factory('$pushService', ['$window', function ($localStorage) {
    return {
        onNotification: function (e) {

            console.log("$cordovaPush:notificationReceived");

            switch (e.event) {
                case 'registered':
                    if (e.regid.length > 0) {
                        $("#app-status-ul").append('<li>REGISTERED -> REGID:' + e.regid + "</li>");
                        // Your GCM push server needs to know the regID before it can push to this device
                        // here is where you might want to send it the regID for later use.
                        console.log(e);
                        $localStorage.setDeviceToken(e.regid);
                    }
                    break;

                case 'message':

                    // if this flag is set, this notification happened while we were in the foreground.
                    if (e.foreground) {
                        console.log("foreground notification");
                    }
                    else {	// otherwise we were launched because the user touched a notification in the notification tray.
                        if (e.coldstart)
                            console.log("coldstart notification");
                        else
                            console.log("background notification");
                    }

                    console.log(e.payload.message);
                    console.log(e.payload.msgcnt);

                    break;

                case 'error':
                    console.log(e.msg);
                    break;

                default:
                    console.log("unknown event" + JSON.stringify(e));
                    break;
            }
        },
    }
}]);