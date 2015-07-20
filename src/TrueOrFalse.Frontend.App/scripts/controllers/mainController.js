app.controller("mainController", function ($rootScope, $scope, $location, $localstorage, $msgService) {

    $scope.userName = $localstorage.getUserName();
    $scope.isDevmode = false;
    $scope.items = $msgService.getAll();

    $rootScope.$watch("msgs", function (newValue, oldValue) {
        $scope.items = newValue;
    });

    $scope.logout = function () {
        $localstorage.setAccessToken("");
        $localstorage.setUserName("");
        $location.path("/login");
    }

    $scope.devpage = function() {
        $location.path("/dev");
    }
});