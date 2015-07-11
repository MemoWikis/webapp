app.factory('$pushService', function ($cordovaPush, $rootScope, $localstorage) {
    return {

        register: function () {

            console.log("$pushService:register()");

            var androidConfig = {
                "senderID": settings.androidApiProjectId
            };

            $cordovaPush.register(androidConfig).then(function (deviceToken) {
                console.log("success register cordovaPush: " + deviceToken);
            }, function (err) {
                console.log(err);
            });

            $rootScope.$on('$cordovaPush:notificationReceived', function(x, notification) {
                console.log("$pushService:notificationReceived()");

                switch (notification.event) {
                case 'registered':
                    if (notification.regid.length > 0) {
                        // Your GCM push server needs to know the regID before it can push to this device
                        // here is where you might want to send it the regID for later use.
                        console.log(notification);
                        $localstorage.setDeviceToken(notification.regid);
                    }
                    break;

                case 'message':

                    // if this flag is set, this notification happened while we were in the foreground.
                    if (notification.foreground) {
                        console.log("foreground notification");
                    } else { // otherwise we were launched because the user touched a notification in the notification tray.
                        if (notification.coldstart)
                            console.log("coldstart notification");
                        else
                            console.log("background notification");
                    }

                    console.log(notification.payload.message);
                    console.log(notification.payload.msgcnt);

                    break;

                case 'error':
                    console.log(notification.msg);
                    break;

                default:
                    console.log("unknown event" + JSON.stringify(notification));
                    break;
                }
            });
        }
    }
});