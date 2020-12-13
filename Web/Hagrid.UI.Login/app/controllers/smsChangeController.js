'use strict';
accountsApp.controller('smsChangeController', ['$scope', '$sessionStorage', '$routeParams', '$location', '$window', '$timeout',
    'authService', 'smsService', 'SystemMessages',
    function ($scope, $sessionStorage, $routeParams, $location, $window, $timeout, authService, smsService, SystemMessages) {

        $scope.isLoaded = false;
        $scope.message = null;
        $scope.messageClass = null;
        $scope.systemMessages = SystemMessages;
        $scope.showFormRecovery = true;

        $scope.showMessage = function (msgErro) {
            var msgObj = SystemMessages[msgErro];
            if (typeof(msgObj) == 'undefined' && msgErro.messages) {
                if (msgErro.messages.length > 0) {
                    msgObj = {
                        text: msgErro.messages[0].text,
                        css: 'alert-danger'
                    };
                }
            } else if (typeof(msgObj) == 'undefined' && msgErro.Message) {
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

        };

        $scope.login = function () {
            $location.path("login");
        };

        $scope.submit = function () {
            console.log('submit');
            if ($scope.email != $scope.emailnew) {
                console.log('email_different');
                $scope.showMessage('email_different');
                return false;
            }

            if ($scope.password != $scope.passwordnew) {
                console.log('password_different');
                $scope.showMessage('password_different');
                return false;
            }

            smsService.resetAccountChange($scope.emailnew, $scope.passwordnew).then(function (response) {
                $scope.showMessage('success');
                $scope.showFormRecovery = false;

                delete $sessionStorage.AuthToken;

                var ub = $sessionStorage.urlBack;

                if (typeof(ub) != "undefined") {
                    delete $sessionStorage.urlBack;
                    delete $sessionStorage.applications;

                    ub = ub.indexOf('http') > -1 ? ub : 'http://' + ub;
                    $timeout(function () { $window.location.href = ub }, 5000);
                }
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