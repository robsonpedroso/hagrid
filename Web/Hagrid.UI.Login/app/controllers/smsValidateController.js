'use strict';
accountsApp.controller('smsValidateController', ['$scope', '$sessionStorage', '$routeParams', '$location', '$window', '$timeout', 'authService', 'smsService', 'SystemMessages',
    function ($scope, $sessionStorage, $routeParams, $location, $window, $timeout, authService, smsService, SystemMessages) {

    $scope.isLoaded = false;
    $scope.displayed = null;
    $scope.message = null;
    $scope.messageClass = null;

    $scope.showMessage = function (msgErro) {
        var msgObj = SystemMessages[msgErro];
        if (typeof (msgObj) == 'undefined' && msgErro.messages) {
            if (msgErro.messages.length > 0) {
                msgObj = {
                    text: msgErro.messages[0].text,
                    css: 'alert-danger'
                };
            }
        } else if (typeof (msgObj) == 'undefined' && msgErro.Message) {
            msgObj = {
                text: msgErro.Message,
                css: 'alert-danger'
            };
        }

        if (typeof (msgObj) != 'undefined') {
            $scope.message = msgObj.text;
            $scope.messageClass = msgObj.css;
            $scope.visibleMessage = 'message';
        }
    };

    $scope.init = function () {
        if ($routeParams.ub) {
            $sessionStorage.urlBack = $routeParams.ub;
            $sessionStorage.$apply();
        }
    };

    $scope.newCode = function () {
        var user = $sessionStorage.smsUser;

        if (typeof(user) == "undefined") {
            $location.path("sms-recovery");
            return;
        }

        var ub = $sessionStorage.urlBack;
        if (typeof(ub) == "undefined") {
            ub = '';
        }

        smsService.resetAccountToken(user.email, user.document, ub).then(function (response) {
            $scope.showMessage('success_resend');

            $timeout(function () { $window.location.href = response.content.token_validation_url }, 1300);
        },
            function (err) {
                if (err.data && err.data.error)
                    $scope.showMessage(err.data.error);
                else
                    $scope.showMessage(err.data);
            });

    };

    $scope.submit = function () {
        if ($scope.code == "") {
            $scope.messageClass = 'alert-danger';
            $scope.message = "É necessário preencher o campo com o código recebido no celular.";
            $scope.visibleMessage = 'message';

            return false;
        }

        smsService.resetAccountValidate($routeParams.token, $scope.code).then(function (response) {
            $scope.showMessage('success');

            $sessionStorage.AuthToken = response; 
            $sessionStorage.$apply();

            $location.path("sms-change");
        },
            function (err) {
                if (err.status == 401)
                    $location.path("login");

                if (err.data && err.data.error)
                    $scope.showMessage(err.data.error);
                else
                    $scope.showMessage(err.data);

            });
    };

}]);