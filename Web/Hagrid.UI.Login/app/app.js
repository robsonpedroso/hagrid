
var accountsApp = angular.module('accountsApp', ['ngRoute', 'ngStorage', 'ngMessages', 'ngMask', 'accountsApp.constants', 'accountsApp.SystemMessages', 'ngSanitize']);

accountsApp.config(['$routeProvider', '$locationProvider', '$httpProvider', function ($routeProvider, $locationProvider, $httpProvider) {

    $routeProvider.when("/password/recovery/:token", {
        controller: "recoveryController",
        templateUrl: "/app/views/recovery.html"
    });

    $routeProvider.when("/password/change/:token", {
        controller: "changeController",
        templateUrl: "/app/views/change.html"
    });

    $routeProvider.when("/notfound", {
        controller: "notfoundController",
        templateUrl: "/app/views/notfound.html"
    });

    $routeProvider.when("/login/", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/login/:application", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/request-recovery/", {
        controller: "requestRecoveryController",
        templateUrl: "/app/views/request-recovery.html"
    });

    $routeProvider.when("/new-store/notification", {
        controller: "importNotificationController",
        templateUrl: "/app/views/import-notification.html"
    });

    $routeProvider.when("/new-store/notification/:store", {
        controller: "importNotificationController",
        templateUrl: "/app/views/import-notification.html"
    });

    $routeProvider.when("/new-store/password/recovery/:token", {
        controller: "recoveryController",
        templateUrl: "/app/views/recovery.html"
    });

    $routeProvider.when("/credit-card/:store/:phrase", {
        controller: "creditCardController",
        templateUrl: "/app/views/creditcard.html"
    });

    $routeProvider.when("/sms-recovery/", {
        controller: "smsRecoveryController",
        templateUrl: "/app/views/sms-recovery.html"
    });

    $routeProvider.when("/sms-validate/:token", {
        controller: "smsValidateController",
        templateUrl: "/app/views/sms-validate.html"
    });

    $routeProvider.when("/sms-change/", {
        controller: "smsChangeController",
        templateUrl: "/app/views/sms-change.html"
    });

    $routeProvider.otherwise({ redirectTo: "/notfound" });

    $httpProvider.defaults.headers.put['Content-Type'] = 'application/x-www-form-urlencoded';
    $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
    $httpProvider.interceptors.push('authInterceptorService');

}]);

accountsApp.directive('ngPasswordValidation', ['authService', '$location', function (authService, $location) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            ele.on('blur', function () {
                if (c.$viewValue) {

                    var isCustomerImport = $location.url().indexOf('new-store') > -1;

                    authService.validatePassword(c.$viewValue, isCustomerImport).then(function (response) {

                        angular.forEach(c.$error, function (value, key) {
                            c.$setValidity(key, true);
                        });

                        if (response) {

                            angular.forEach(response, function (value, key) {
                                c.$setValidity(value, false);
                            });
                        }
                    },
                    function (err) {
                        c.$setValidity('generic', false);
                        console.log(err);
                    });
                }
            });
        }
    };
}]);

accountsApp.directive('ngPasswordConfirmation', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, ele, attrs, c) {
            ele.on('blur', function () {
                c.$setValidity('different', c.$viewValue == scope[attrs.ngPasswordConfirmation]);
            });
        }
    };
});

accountsApp.directive('ngSubmitValidation', function () {
    return {
        restrict: 'A',
        link: function (scope, ele, attrs, c) {
            ele.on('click', function () {

                if (scope[attrs.ngSubmitValidation].$invalid) {
                    angular.forEach(scope[attrs.ngSubmitValidation].$error, function (field) {
                        angular.forEach(field, function (errorField) {
                            errorField.$setTouched();
                        })
                    });
                }
            });
        }
    };
});

accountsApp.directive('ngLoadingButton', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            element.on('click', function ($event) {

                scope.started = false;

                scope.isLoading = function () {
                    return $http.pendingRequests.length;
                };

                var unbindWatcher = scope.$watch(scope.isLoading, function (value) {
                    if (value == 1 && !scope.started) {
                        element.parent().append('<div id="loading-bar-spinner"><div class="spinner-icon"></div></div>');
                        element.addClass('ng-hide');
                        scope.started = true;
                    } else if (value == 0 && scope.started) {
                        element.parent().find('#loading-bar-spinner').remove();
                        element.removeClass('ng-hide');
                        scope.started = false;
                        unbindWatcher();
                    }
                });

            });
        }
    };
}]);

accountsApp.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

accountsApp
    .filter('currentyear', function () {
        return function (x) {
            return new Date().getFullYear()
        };
    });