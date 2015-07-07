app.factory('$deviceInfo', function () {
    return {
        getJson: function () {
            return {
                DeviceInformation : ionic.Platform.device(),
                IsWebView : ionic.Platform.isWebView(),
                IsIPad : ionic.Platform.isIPad(),
                IsIOS : ionic.Platform.isIOS(),
                IsAndroid : ionic.Platform.isAndroid(),
                IsWindowsPhone : ionic.Platform.isWindowsPhone(),

                CurrentPlatform : ionic.Platform.platform(),
                CurrentPlatformVersion : ionic.Platform.version()
            }
        },
    }
});