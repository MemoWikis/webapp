app.controller("loginController", function ($scope, $location, $http, $localstorage, $ionicLoading, $deviceInfo) {

    $scope.user = {};

    $scope.login = function () {

        $ionicLoading.show({
            template: '<ion-spinner icon="ripple"/>'
        });

        var appInfo = $deviceInfo.getJson();
        $http.post(settings.Url_GetLoginToken(), {
            userName: $scope.user.email,
            password: $scope.user.password,
            appName: "MEMO1",
            appInfoJson: JSON.stringify(appInfo),
            deviceKey: $localstorage.getDeviceToken()
        }).success(function(result) {
            console.log(result.AccessToken);
            console.log(result.LoginSuccess);

            if (result.LoginSuccess) {
                $localstorage.setAccessToken(result.AccessToken);
                $localstorage.setUserName(result.UserName);
                $ionicLoading.hide();
                $location.path("/main");

                return;
            } else {
                console.log(result);
                $ionicLoading.hide();
                $scope.hasError = true;
            }
        }).error(function (result) {
            console.log(result);
            $ionicLoading.hide();
            $scope.hasError = true;
        });
    };
});