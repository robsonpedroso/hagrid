'use strict';
accountsApp.controller('smsRecoveryController', ['$scope', '$sessionStorage', '$routeParams', '$location', '$window', '$timeout', 'authService', 'smsService', 'SystemMessages',
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

            authService.getClientCredentialToken().then(function (response) {

                if (typeof(response.access_token) !== "undefined" && response.access_token != "") {

                    if ($routeParams.ub) {
                        $sessionStorage.urlBack = $routeParams.ub;
                        $sessionStorage.$apply();
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

        $scope.login = function () {
            $location.path("login");
        };

        $scope.submit = function () {
            if ($scope.email == "" && $scope.document == "") {
                showMessage('email_document_required');
                return false;
            }

            var ub = $sessionStorage.urlBack;
            if (typeof (ub) == "undefined") {
                ub = '';
            }

            smsService.resetAccountToken($scope.email, $scope.document, ub).then(function (response) {
                $scope.showMessage('success');

                $scope.smsUser = {
                    email: $scope.email,
                    document: $scope.document
                };

                $sessionStorage.smsUser = $scope.smsUser;
                $sessionStorage.$apply();

                $window.location.href = response.content.token_validation_url;
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