(function () {
    'use strict';

    function themeService($http) {

        var serviceRoot = Umbraco.Sys.ServerVariables.application.applicationPath + 'umbraco/backoffice/themes/backOfficeThemeApi/';

        var service = {
            getThemes: getThemes,
            applyTheme: applyTheme,
            resetTheme: resetTheme,
            getCurrentTheme: getCurrentTheme,
            getCurrentThemeInfo: getCurrentThemeInfo

        };

        return service; 

        function getThemes() {
            return $http.get(serviceRoot + 'GetThemes');
        }

        function applyTheme(alias) {
            return $http.put(serviceRoot + 'ApplyTheme/?alias=' + alias);
        }

        function resetTheme() {
            return $http.put(serviceRoot + 'ResetTheme');
        }

        function getCurrentTheme() {
            return $http.get(serviceRoot + 'GetCurrentTheme');
        }

        function getCurrentThemeInfo() {
            return $http.get(serviceRoot + 'GetCurrentThemeInfo');
        }

    }


    angular.module('umbraco.services')
        .factory('backofficeThemeService', themeService);
})();