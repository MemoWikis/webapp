app.controller("devController", function (
    $rootScope,
    $scope,
    $location,
    $localstorage,
    $deviceInfo,
    $msgService,
    $pushService) {

    $scope.userName = $localstorage.getUserName();
    $scope.accessToken = $localstorage.getAccessToken();
    $scope.deviceToken = $localstorage.getDeviceToken();
    $scope.messageCount = $msgService.getAll().length;
    $scope.senderId = settings.androidApiProjectId;


    $rootScope.$watch("msgs", function(newValue, oldValue) {
        $scope.messageCount = newValue.length;
    });

    $scope.back = function () {
        $location.path("/main");
    }

    $scope.pushRegister = function () {
        $pushService.register();
    }

    $scope.createMsg = function () {
        var newMsg = msg.create();
        newMsg.text = "Jetzt wiederholen!";
        $msgService.add(newMsg);
    }

    $scope.deleteMsgs = function() {
        $msgService.deleteAll();
    }

    $scope.clearLocalStorage = function () {
        $localstorage.clear();
    }

});