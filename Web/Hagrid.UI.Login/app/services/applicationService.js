'use strict';
accountsApp
    .factory('applicationService', ApplicationService);

ApplicationService.$inject = ['$http', '$q', 'Config', '$sessionStorage'];

function ApplicationService($http, $q, $config, $sessionStorage) {

    var vm = this;
    vm.get = get;
    vm.info = info;

    function get(name) {

        var deferred = $q.defer();
        var errCallback = function (err) {
            delete $sessionStorage.AuthToken;
            deferred.reject(err);
        };

        $http.get($config.baseURI + '/v2/application/' + name).then(function (response) {
            deferred.resolve(response.data);
        },
            errCallback);

        return deferred.promise;
    }

    function info(application) {
        var result = {};
        var deferred = $q.defer();

        if (typeof ($sessionStorage.applications) !== 'undefined' &&
            ($sessionStorage.applications == application || typeof (application) == 'undefined'))
        {
            if ($sessionStorage.applications.informations.html_content)
                result.HtmlContent = $sessionStorage.applications.informations.html_content;

            if ($sessionStorage.applications.informations.image)
                result.Logo = $sessionStorage.applications.informations.image;

            result.Title = $sessionStorage.applications.informations.title;
            document.title = $sessionStorage.applications.informations.title;

            deferred.resolve(result);
        }
        else {

            if (typeof (application) !== 'undefined') {

                delete $sessionStorage.applications;

                get(application).then(function (response) {
                    if (typeof (response.content) !== 'undefined') {
                        $sessionStorage.applications = response.content;

                        if (typeof(response.content.informations) !== 'undefined') {

                            if (response.content.informations.html_content)
                                result.HtmlContent = response.content.informations.html_content;

                            if (response.content.informations.image)
                                result.Logo = response.content.informations.image;

                            result.Title = response.content.informations.title;
                            document.title = response.content.informations.title;
                        }
                    }

                    deferred.resolve(result);
                });
            }
            else
                deferred.resolve(result);
        }
        
        return deferred.promise;
    };

    return vm;
};