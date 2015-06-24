app.controller("loginController", function ($scope, $location) {

    $scope.user = {};

    $scope.login = function () {

        var email = $scope.user.email;
        var password = $scope.user.password;

        if (email == "test@test.de" && password == "test") {
            $location.path("/main");
            return;
        }

        $scope.hasError = true;
    };
});