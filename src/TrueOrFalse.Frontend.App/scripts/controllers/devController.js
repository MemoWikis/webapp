app.controller("devController", function ($scope, $location, $localstorage) {

    $scope.userName = $localstorage.getUserName();
    $scope.accessToken = $localstorage.getAccessToken();

    console.log("userName", $localstorage.getUserName());

    $scope.back = function () {
        $location.path("/main");
    }

});