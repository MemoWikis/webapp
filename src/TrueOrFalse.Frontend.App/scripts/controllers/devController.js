app.controller("devController", function ($scope, $location, $localstorage, $deviceInfo, $msgService) {

    $scope.userName = $localstorage.getUserName();
    $scope.accessToken = $localstorage.getAccessToken();
    $scope.deviceToken = $localstorage.getDeviceToken();
    $scope.messageCount = $msgService.getAll().length;

    $scope.back = function () {
        $location.path("/main");
    }

    $scope.pushInfo = function() {
        alert("huuray");
    }

    $scope.createMsg = function () {
        var newMsg = msg.create();
        newMsg.text = "Jetzt wiederholen!";
        $msgService.add(newMsg);

        $scope.messageCount = $msgService.getAll().length;
    }

    $scope.deleteMsgs = function() {
        $msgService.deleteAll();
        $scope.messageCount = $msgService.getAll().length;
    }

});