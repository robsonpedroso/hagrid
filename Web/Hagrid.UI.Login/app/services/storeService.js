'use strict';
accountsApp.factory('storeService', ['$http', '$q', 'Config', function ($http, $q, $config) {

    var storeService = {};

    storeService.getStore = function (code) {

        var deferred = $q.defer();
        var errCallback = function (err) {
            delete $sessionStorage.AuthToken;
            deferred.reject(err);
        };

        $http.get($config.baseURI + '/store/' + code).then(function (response) {
            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    }

    return storeService;

}]);