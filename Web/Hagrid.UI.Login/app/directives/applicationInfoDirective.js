'use strict';
accountsApp
    .controller('appInfoController', AppInfoController)
    .directive('appInfo', AppInfoDirective);

AppInfoController.$inject = ['$scope', '$routeParams', 'applicationService'];

function AppInfoController($scope, $routeParams, applicationService) {
    $scope.init = init;
    $scope.isLoaded = false;
    $scope.infoApp = {};

    function init() {
        applicationService.info($routeParams.application).then(function (response) {
            $scope.infoApp = response;
            $scope.isLoaded = true;
        });
    }
};


function AppInfoDirective() {
    return {
        templateUrl: 'app/directives/app-info.html',
        controller: AppInfoController
    };
};