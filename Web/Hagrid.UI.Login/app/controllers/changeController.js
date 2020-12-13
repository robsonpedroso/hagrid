'use strict';
accountsApp.controller('changeController', ['$scope', '$sessionStorage', '$routeParams', '$timeout', 'authService',
    function ($scope, $sessionStorage, $routeParams, $timeout, authService) {

    $scope.isValid = false;
    $scope.isLoaded = false;
    $scope.displayed = null;
    $scope.message = null;
    $scope.messageClass = null;
    $scope.password = null;
    $scope.passwordConf = null;
    $scope.passwordNew = null;
    $scope.clientAppLogo = null;
    $scope.showChangePasswordMessage = null;

    $scope.messages = {
        'invalid_token': {
            text: 'Esse link de alteração de senha é inválido, por favor tente novamente.',
            cssClass: 'alert-danger'
        },
        'locked_member': {
            text: 'Seu usuário está bloqueado temporiariamente por tentativas sucessivas de login inválidas.',
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

        authService.changePasswordToken($routeParams.token).then(function (response) {
            $scope.isValid = true;
            $scope.displayed = 'form';
            $scope.isLoaded = true;
            $scope.clientAppLogo = response.client_logo_url;
            $scope.showChangePasswordMessage = response.change_password_message;
        },
        function (err) {
            $scope.isValid = false;
            $scope.showMessage('invalid_token');
            $scope.isLoaded = true;

            delete $sessionStorage.AuthToken;
        });
    };

    $scope.changePassword = function () {
        authService.changePassword($scope.password, $scope.passwordNew).then(function (response) {
            $scope.showMessage('success');

            if($sessionStorage.AuthToken.url_back) {
                var urlBack = $sessionStorage.AuthToken.url_back;
                $timeout(function () { window.location.href = decodeURIComponent(urlBack); }, 1500);
            }

            delete $sessionStorage.AuthToken;
        },
        function (err) {

            if (err) {

                err.Message = err.Message ? err.Message : (err.data ? err.data.Message : null);

                if(err.Message) {
                    if (err.Message == 'invalid_password') {
                        $scope.pwdForm.password.$setValidity("invalid_password", false);
                    }
                    else if (err.Message == 'locked_member') {
                        delete $sessionStorage.AuthToken;
                        $scope.showMessage('locked_member');
                    }
                }
            }
            else {
                $scope.showMessage('fail');
            }
        });
    };

    $scope.setPasswordValidity = function () {
        $scope.pwdForm.password.$setValidity("invalid_password", true);
    };

}]);