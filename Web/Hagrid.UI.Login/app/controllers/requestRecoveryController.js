'use strict';
accountsApp.controller('requestRecoveryController', ['$scope', '$routeParams', '$timeout', '$location', '$sessionStorage', '$window', 'authService',
    function ($scope, $routeParams, $timeout, $location, $sessionStorage, $window, authService) {

        $scope.isLoaded = false;
        $scope.visibleMessage = null;
        $scope.message = null;
        $scope.messageClass = null;
        $scope.email = null;
        $scope.applicationHtmlContent = null;

        $scope.messages = {
            'locked_member': {
                text: 'Seu usuário está bloqueado temporiariamente por tentativas sucessivas de login inválidas.',
                cssClass: 'alert-danger'
            },
            'success': {
                text: 'Solicitação enviada com sucesso! Você receberá uma solicitação de troca de senha no seu e-mail.',
                cssClass: 'alert-success'
            },
            'invalid_grant': {
                text: 'Usuário e / ou senha inválidos',
                cssClass: 'alert-danger'
            }
        };

        $scope.showMessage = function (messageId) {


            var messageObject = $scope.messages[messageId];

            if (messageObject) {

                $scope.messageClass = messageObject.cssClass;
                $scope.message = messageObject.text;

            }  else {
                $scope.messageClass = 'alert-danger';
                $scope.message = messageId;
            }

            $scope.visibleMessage = true;
        };

        $scope.doRequestRecovery = function () {

            var ub = $sessionStorage.urlBack;

            if (typeof (ub) == "undefined") {
                ub = '';
            }

            authService.resetPasswordEmail($scope.email, ub).then(function (response) {

                $scope.email = "";
                $scope.requestRecoveryForm.email.$touched = false;
                
                
                $scope.showMessage(response.messages[0].type.toLowerCase());

                if (ub != '') {
                    delete $sessionStorage.urlBack;
                }
            },
            function (err) {

                console.log(err);

                if (err.data.error) {
                    $scope.showMessage(err.data.error);
                } else {
                    $scope.showMessage(err.data.messages[0].text);
                }
                $scope.isLoaded = true;
            });

        };

        $scope.login = function () {
            $location.path("login");
        };

        $scope.lostAccount = function () {
            $location.path("sms-recovery");
        };

        $scope.init = function () {

            authService.getClientCredentialToken().then(function (response) {

                if (typeof (response.access_token) !== "undefined" && response.access_token != "") {

                    if ($routeParams.ub) {
                        $sessionStorage.urlBack = $routeParams.ub;
                        $location.search({});
                    }

                    $scope.displayed = 'form';
                    $scope.isLoaded = true;
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

    }]);