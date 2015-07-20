var app = angular.module('starter', ['ionic', 'ngCordova']);

(function () {
    "use strict";
    //$pushService
    app.run(function ($ionicPlatform, $pushService, $rootScope, $msgService) {
        $ionicPlatform.ready(function () {
            if (window.StatusBar) {
                StatusBar.styleDefault();
            }

            $rootScope.msgs = $msgService.getAll();

        });

        document.addEventListener("deviceready", function () {

            window.setTimeout(function() {
                // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
                // for form inputs)
                if (window.cordova && window.cordova.plugins != null && window.cordova.plugins.Keyboard) {
                    cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
                }

                if (!ionic.Platform.isWebView()) {
                    $pushService.register();
                }

            }, 1000);

        });
    });

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener('pause', onPause.bind(this), false);
        document.addEventListener('resume', onResume.bind(this), false);
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };
})();