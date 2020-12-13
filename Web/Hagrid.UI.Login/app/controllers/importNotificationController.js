'use strict';
accountsApp.controller('importNotificationController', ['$scope', '$routeParams', 'storeService', function ($scope, $routeParams, storeService) {

    $scope.isValid = false;
    $scope.isLoaded = false;
    $scope.storeLogo = null;
    $scope.storeName = null;

    $scope.init = function () {

        if ($routeParams.store) {

            storeService.getStore($routeParams.store).then(function (response) {

                $scope.storeLogo = response.content.logo;
                $scope.storeName = response.content.name;

                $scope.isValid = true;
                
            });
        } 

        $scope.isLoaded = true;

    };

}]);