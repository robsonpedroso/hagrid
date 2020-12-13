'use strict';
accountsApp.controller('loginController', ['$scope', '$sessionStorage', '$routeParams', '$timeout', '$location', '$window', 'authService',
    function ($scope, $sessionStorage, $routeParams, $timeout, $location, $window, authService) {

        $scope.isLoaded = false;
        $scope.visibleMessage = null;
        $scope.message = null;
        $scope.messageClass = null;
        $scope.user = null
        $scope.password = null;
        

        $scope.messages = {
            'locked_member': {
                text: 'Seu usuário está bloqueado temporiariamente por tentativas sucessivas de login inválidas.',
                cssClass: 'alert-danger'
            },
            'success': {
                text: 'Sua senha foi alterada com sucesso.',
                cssClass: 'alert-success'
            },
            'invalid_grant': {
                text: 'Usuário e / ou senha inválidos',
                cssClass: 'alert-danger'
            }
        };

        $scope.showMessage = function (messageId) {

            var messageObject = $scope.messages[messageId];

            $scope.messageClass = messageObject.cssClass;
            $scope.message = messageObject.text;
            $scope.visibleMessage = true;
        };

        $scope.doLogin = function () {
            authService.login($scope.user, $scope.password).then(function (response) {

                var ub = $sessionStorage.urlBack;

                if (ub != null) {

                    authService.transferToken(response.access_token).then(function (responseTT) {

                        delete $sessionStorage.urlBack;
                        delete $sessionStorage.AuthToken;
                        delete $sessionStorage.applications;

                        $window.location.href = ub + responseTT.content.transfer_token;
                    });
                }
                else {
                    $location.path("notfound");
                }
            },
            function (err) {

                $scope.showMessage(err.data.error);
                $scope.isLoaded = true;
            });
        };

        $scope.getUrlBack = function () {
            var ub = $location.search().ub;

            if (typeof (ub) !== "undefined") {
                $sessionStorage.urlBack = ub;
                $location.search({});
                delete $sessionStorage.applications;
            } else if ($sessionStorage.urlBack == null) {
                delete $sessionStorage.applications;
                $location.path("notfound");
            }
        };

        $scope.lostPassword = function () {
            $location.path("request-recovery");
        };

        $scope.init = function () {
            $scope.getUrlBack();
            $scope.displayed = 'form';
            $scope.isLoaded = true;
        };

    }]);