app.controller("devController", function ($scope, $location, $localstorage, $deviceInfo) {

    $scope.userName = $localstorage.getUserName();
    $scope.accessToken = $localstorage.getAccessToken();
    $scope.deviceToken = $localstorage.getDeviceToken();

    console.log("deviceInfo", $deviceInfo.getJson());

    $scope.back = function () {
        $location.path("/main");
    }

    $scope.pushInfo = function() {
        alert("huuray");
    }

});