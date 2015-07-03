app.controller("mainController", function ($scope, $location, $localstorage) {

    $scope.userName = $localstorage.getUserName();
    $scope.isDevmode = false;

    $scope.logout = function () {
        $localstorage.setAccessToken("");
        $localstorage.setUserName("");
        $location.path("/login");
    }

    $scope.devpage = function() {
        $location.path("/dev");
    }
});