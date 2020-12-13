'use strict';

accountsApp.factory('authService', ['$http', '$q', '$sessionStorage', 'Config', function ($http, $q, $sessionStorage, Config) {

    var authServiceFactory = {};

    authServiceFactory.getClientCredentialToken = function () {
        var deferred = $q.defer();
        var errCallback = function (err) {
            delete $sessionStorage.AuthToken;
            deferred.reject(err);
        };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var data = "grant_type=client_credentials&client_id=" + Config.clientId;

        $http.post(Config.baseURI + '/token', data, headers).then(function (response) {

            $sessionStorage.AuthToken = response.data;
            $sessionStorage.$apply();

            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    }

    authServiceFactory.resetPasswordEmail = function (email, urlBack) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };

        var data = 'email=' + email + '&url_back=' + urlBack;

        $http.post(Config.baseURI + '/v2/member/password-reset/email/', data, headers).then(function (response) {

            deferred.resolve(response.data); 
        },
        errCallback);

        return deferred.promise;
    };

    authServiceFactory.resetPasswordToken = function (token) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var data = 'grant_type=reset_password&reset_token=' + token + '&client_id=' + Config.clientId;

        if (!$sessionStorage.AuthToken) {

            $http.post(Config.baseURI + '/token', data, headers).then(function (response) {

                $sessionStorage.AuthToken = response.data;
                deferred.resolve(response.data);
            },
            errCallback);
        }
        else {
            deferred.resolve($sessionStorage.AuthToken);
        }

        return deferred.promise;
    };

    authServiceFactory.resetPassword = function (newPassword, isCustomerImport) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };

        var newPwd = encodeURIComponent(newPassword);
        var data = 'password=' + newPwd;

        var route = isCustomerImport ? '/member-import/password-reset' : '/member/password-reset';

        $http.post(Config.baseURI + route, data, headers).then(function (response) {
            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    };

    authServiceFactory.changePasswordToken = function (token) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var data = 'grant_type=change_password&change_token=' + token + '&client_id=' + Config.clientId;

        $http.post(Config.baseURI + '/token', data, headers).then(function (response) {

            $sessionStorage.AuthToken = response.data;
            deferred.resolve(response.data);
        }, 
        errCallback);

        return deferred.promise;
    };

    authServiceFactory.changePassword = function (password, newPassword) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var newPwd = encodeURIComponent(newPassword);
        var pwd = encodeURIComponent(password);
         
        var data = 'password=' + pwd + '&password_new=' + newPwd;

        $http.post(Config.baseURI + '/member/password-change', data, headers).then(function (response) {
            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    };

    authServiceFactory.validatePassword = function (newPassword, isCustomerImport) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var newPwd = encodeURIComponent(newPassword);

        var data = 'password=' + newPwd;

        var route = isCustomerImport ? '/member-import/password-validate' : '/member/password-validate';

        $http.post(Config.baseURI + route, data, headers).then(function (response) {
            if (isCustomerImport)
                deferred.resolve(response.data.content);
            else
                deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    };

    authServiceFactory.login = function (user, password) {

        var deferred = $q.defer(); 
        var errCallback = function (err) {

            delete $sessionStorage.AuthToken;

            deferred.reject(err);
        };

        var pwd = encodeURIComponent(password);

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var data = "grant_type=password&username=" + user + "&password=" + pwd + "&client_id=" + Config.clientId;

        $http.post(Config.baseURI + '/token', data, headers).then(function (response) {

            $sessionStorage.AuthToken = response.data;
            $sessionStorage.$apply();

            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;

    };

    authServiceFactory.transferToken = function (memberToken) {

        var deferred = $q.defer();
        var errCallback = function (err) {

            deferred.reject(err);
        };

        $http.get(Config.baseURI + '/v2/member/get-transfer-token').then(function (response) {

            deferred.resolve(response.data);
        },
        errCallback);

        return deferred.promise;
    };

    return authServiceFactory;
}]);