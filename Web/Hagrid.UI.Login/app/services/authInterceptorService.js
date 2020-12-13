'use strict';
accountsApp.factory('authInterceptorService', ['$q', '$injector', '$location', '$sessionStorage', function ($q, $injector, $location, $sessionStorage) {

    var authInterceptorServiceFactory = {};

    authInterceptorServiceFactory.request = function (config) {

        config.headers = config.headers || {};

        var token = $sessionStorage.AuthToken;

        if (token) {
            config.headers.Authorization = 'Bearer ' + token.access_token;
        }

        return config;
    };

    return authInterceptorServiceFactory;
}]);