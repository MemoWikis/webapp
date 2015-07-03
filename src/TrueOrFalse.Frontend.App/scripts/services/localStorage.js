app.factory('$localstorage', ['$window', function ($window) {
    return {
        set: function (key, value) {
            $window.localStorage[key] = value;
        },
        get: function (key, defaultValue) {
            return $window.localStorage[key] || defaultValue;
        },
        setObject: function (key, value) {
            $window.localStorage[key] = JSON.stringify(value);
        },
        getObject: function (key) {
            return JSON.parse($window.localStorage[key] || '{}');
        },
        setAccessToken: function (value) {
            return this.set("accessToken", value);
        },
        getAccessToken: function () {
            return this.get("accessToken");
        },
        setUserName: function (value) {
            return this.set("userName", value);
        },
        getUserName: function () {
            return this.get("userName");
        },
        setDeviceToken: function (value) {
        return this.set("deviceToken", value);
        },
        getDeviceToken: function () {
            return this.get("deviceToken");
        }
    }
}]);