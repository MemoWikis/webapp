app.controller("loginController", function ($scope, $location, $http, $localstorage, $ionicLoading) {

    $scope.user = {};

    $scope.login = function () {

        $ionicLoading.show({
            template: '<ion-spinner icon="ripple"/>'
        });

        $http.post("http://memucho/app/GetLoginToken", {
            userName: $scope.user.email,
            password: $scope.user.password,
            appName: "MEMO1"
        }).success(function(result) {
            console.log(result.AccessToken);
            console.log(result.LoginSuccess);

            if (result.LoginSuccess) {
                $localstorage.setAccessToken(result.AccessToken);
                $localstorage.setUserName(result.UserName);
                $ionicLoading.hide();
                $location.path("/main");

                //send device id to MEMuchO
                return;
            } else {
                $scope.hasError = true;
            }
        }).error(function (result) {
            console.log(result);
            $ionicLoading.hide();
            $scope.hasError = true;
        });
    };
});