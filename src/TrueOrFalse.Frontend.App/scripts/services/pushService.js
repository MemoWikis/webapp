app.factory('$pushService', function (
    $cordovaPush,
    $cordovaVibration,
    $cordovaLocalNotification,
    $rootScope,
    $localstorage,
    $msgService) {
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
                        console.log("regId: " + notification.regid);
                        $localstorage.setDeviceToken(notification.regid);
                    }
                    break;

                case 'message':

                    // if this flag is set, this notification happened while we were in the foreground.
                    if (notification.foreground) {
                        console.log("foreground notification");
                    } else { // otherwise we were launched because the user touched a notification in the notification tray.
                        if (notification.coldstart){
                            console.log("coldstart notification");
                        }else{
                            console.log("background notification");
                        }
                    }

                    console.log(notification.payload.default);
                    console.log(notification.payload.msgcnt);

                    var newMsg = msg.create();
                    newMsg.text = notification.payload.default;
                    $msgService.add(newMsg);

                    $cordovaVibration.vibrate(100);

                    $rootScope.scheduleSingleNotification = function () {
                        $cordovaLocalNotification.schedule({
                            id: 1,
                            title: 'Jetzt Lernen',
                            text: notification.payload.default,
                            data: {
                                customProperty: 'custom value'
                            }
                        }).then(function (result) {
                            console.log(result);
                        });
                    };

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