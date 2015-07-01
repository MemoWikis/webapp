app.controller("mainController", function ($scope, $location, $localstorage) {

    $scope.userName = $localstorage.getUserName();

    $scope.logout = function () {
        $localstorage.setAccessToken("");
        $localstorage.setUserName("");
        $location.path("/login");
    }

});