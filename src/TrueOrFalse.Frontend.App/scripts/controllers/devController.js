app.controller("devController", function ($scope, $location, $localstorage, $cordovaPush) {

    $scope.userName = $localstorage.getUserName();
    $scope.accessToken = $localstorage.getAccessToken();
    $scope.deviceToken = $localstorage.getDeviceToken();

    console.log("userName", $localstorage.getUserName());

    $scope.back = function () {
        $location.path("/main");
    }

    $scope.pushInfo = function() {
        alert("huuray");
    }

});