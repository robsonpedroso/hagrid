'use strict';
accountsApp.controller('recoveryController', ['$scope', '$sessionStorage', '$routeParams', '$location', '$window', '$timeout', 'authService', function ($scope, $sessionStorage, $routeParams, $location, $window, $timeout, authService) {

    $scope.isValid = false;
    $scope.isLoaded = false;
    $scope.displayed = null;
    $scope.message = null;
    $scope.messageClass = null;
    $scope.password = null;
    $scope.passwordConf = null;
    $scope.clientAppLogo = null;
    $scope.isChanged = false;

    $scope.messages = {
        'invalid_token': {
            text: 'Esse link de recuperação de senha é inválido. Caso tenha solicitado a recuperação de senha mais de uma vez, pode ser que há um novo link válido em seu email.',
            cssClass: 'alert-danger'
        },
        'success': {
            text: 'Sua senha foi alterada com sucesso.',
            cssClass: 'alert-success'
        },
        'fail': {
            text: 'Ocorreu algum problema durante a alteração de sua senha. Por favor, tente novamente.',
            cssClass: 'alert-danger'
        }
    };

    $scope.showMessage = function (messageId) {

        var messageObject = $scope.messages[messageId];
        $scope.messageClass = messageObject.cssClass;
        $scope.message = messageObject.text;
        $scope.displayed = 'message';
    };

    $scope.init = function () {

        authService.resetPasswordToken($routeParams.token).then(function (response) {

            if ($routeParams.ub) {
                $sessionStorage.urlBack = $routeParams.ub;
                $location.search({});
            }

            $scope.isValid = true;
            $scope.displayed = 'form';
            $scope.isLoaded = true;
            $scope.clientAppLogo = response.client_logo_url;
        },
        function (err) {
            $scope.isValid = false;
            $scope.showMessage('invalid_token');
            $scope.isLoaded = true;

            delete $sessionStorage.AuthToken;
        });
    };

    $scope.changePassword = function () {
        $scope.isChanged = true;

        var isCustomerImport = $location.url().indexOf('new-store') > -1;

        var ub = $sessionStorage.urlBack;

        if (typeof (ub) == "undefined") {
            ub = '';
        }

        authService.resetPassword($scope.password, isCustomerImport).then(function (response) {
            $scope.showMessage('success');
            delete $sessionStorage.AuthToken;

            if (ub != '') {
                delete $sessionStorage.urlBack;
                delete $sessionStorage.applications;

                ub = ub.indexOf('http') > -1 ? ub : 'http://' + ub;
                $timeout(function () { $window.location.href = ub }, 5000);
            }
        },
        function (err) {
            $scope.showMessage('fail');
            $scope.isChanged = false;
        });
    };

    $scope.submit = function () {

        if ($scope["pwdForm"].$invalid) {
            angular.forEach($scope["pwdForm"].$error, function (field) {
                angular.forEach(field, function (errorField) {
                    errorField.$setTouched();
                })
            });
        }
    };

}]);