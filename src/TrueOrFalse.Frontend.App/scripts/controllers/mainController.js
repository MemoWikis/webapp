app.controller("mainController", function ($scope, $location, $localstorage, $msgService) {

    $scope.userName = $localstorage.getUserName();
    $scope.isDevmode = false;
    $scope.items = $msgService.getAll();

    $scope.logout = function () {
        $localstorage.setAccessToken("");
        $localstorage.setUserName("");
        $location.path("/login");
    }

    $scope.devpage = function() {
        $location.path("/dev");
    }
});