'use strict';
accountsApp.factory('creditCardService', ['$http', '$q', 'Config', function ($http, $q, Config) {

    var creditCardService = {};

    creditCardService.save = function (creditcard) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        $http.defaults.headers.post['Content-Type'] = 'application/json';
        $http.post(Config.baseURI + '/v2/store-credit-card', creditcard).then(function (response) {
            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    }

    creditCardService.getSecretPhrase = function (phrase) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        $http.defaults.headers.post['Content-Type'] = 'application/json';
        $http.get(Config.baseURI + '/v2/store-credit-card/secret-phrase/'+ phrase).then(function (response) {
            deferred.resolve(response);
        },
        errCallback);

        return deferred.promise;
    }

    return creditCardService;

}]);