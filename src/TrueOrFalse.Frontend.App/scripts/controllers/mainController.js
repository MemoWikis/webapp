app.controller("mainController", function ($scope, $location, $localstorage) {

    $scope.logout = function () {
        //$localstorage
        $location.path("/login");
    }

});