'use strict';
accountsApp.factory('smsService', ['$http', '$q', '$sessionStorage', 'Config', function ($http, $q, $sessionStorage, Config) {

    var smsService = {};

    smsService.resetAccountToken = function (email, document, urlBack) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var data = '';

        if (typeof (email) != 'undefined' && email != '')
            data += 'email=' + email + '&';

        if (typeof (document) != 'undefined' && document != '')
            data += 'document=' + document + '&';

        if (typeof(urlBack) != 'undefined' && urlBack != '')
            data += 'url_back=' + urlBack;

        $http.post(Config.baseURI + '/v2/member/password-reset/sms', data, headers).then(function (response) {
            deferred.resolve(response.data);
        },
            errCallback);

        return deferred.promise;
    };

    smsService.resetAccountValidate = function (token, smsCode) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
        var data = 'grant_type=reset_password_sms' +
            '&reset_token=' + token +
            '&sms_code=' + smsCode +
            '&client_id=' + Config.clientId;

        $http.post(Config.baseURI + '/token', data, headers).then(function (response) {

            deferred.resolve(response.data);
        },
            errCallback);

        return deferred.promise;
    };

    smsService.resetAccountChange = function (newEmail, newPassword) {

        var deferred = $q.defer();
        var errCallback = function (err) { deferred.reject(err); };

        var data =
        {
            'email_new': newEmail,
            'password_new': newPassword
        };

        var config = { headers: { 'Content-Type': 'application/json;charset=UTF-8' } };

        var route = '/v2/member/reset-account';
        $http.post(Config.baseURI + route, data, config).then(function (response) {
            deferred.resolve(response.data);
        },
            errCallback);

        return deferred.promise;
    };

    return smsService;
}]);