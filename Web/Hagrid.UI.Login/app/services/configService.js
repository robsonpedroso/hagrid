'use strict';
accountsApp.factory('configService', ['$http', '$q', '$sessionStorage', function ($http, $q, $sessionStorage) {

    var configServiceFactory = {};

    configServiceFactory.Load = function () {

        var deferred = $q.defer();

        $http.get('../app/config.json', { responseType: 'json' }).then(function (response) {

            deferred.resolve(response.data);
        },
        function (err) {

            deferred.resolve({
                "baseURI": "http://localhost:55888",
                "clientId": "04CCCF35-534D-4BF9-A146-53638C054180"
            });
        });

        return deferred.promise;
    };

    configServiceFactory.Current = function () {

        var deferred = $q.defer();

        if ($sessionStorage.Config) {
            deferred.resolve($sessionStorage.Config);
        }
        else {
            configServiceFactory.Load().then(function (response) {

                $sessionStorage.Config = response;
                deferred.resolve(response);
            },
            function (err) {

                deferred.reject(err);
            });
        }

        return deferred.promise;
    };

    return configServiceFactory;

}]);