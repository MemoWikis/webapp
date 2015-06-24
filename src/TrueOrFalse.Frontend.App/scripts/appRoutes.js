app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/');

    $stateProvider.state('login', {
        url: '/login',
        templateUrl: 'view-login.html',
        controller: 'loginController'
    }).state('main', {
        url: '/main',
        templateUrl: 'view-main.html',
        controller: 'mainController'
    });

    $urlRouterProvider.otherwise('/login');

});