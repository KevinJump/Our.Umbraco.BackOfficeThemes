(function () {
    'use strict';

    function themeResource($q, backofficeThemeService) {

        var resource = {
            getCurrentTheme: getCurrentTheme
        };

        return resource;

        ///////////////

        function getCurrentTheme(user) {

            var defer = $q.defer();

            backofficeThemeService.GetCurrentTheme()
                .then(function (result) {
                    defer.resolve(result.data);
                }, function (error) {
                    defer.reject(error);
                });

            defer.promise();
        }
    }

    angular.module('umbraco.resources')
        .factory('themeResource', themeResource);
})();