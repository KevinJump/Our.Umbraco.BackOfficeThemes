(function () {
    'use strict';

    function themeService($http) {

        var serviceRoot = Umbraco.Sys.ServerVariables.application.applicationPath + 'umbraco/backoffice/themes/backOfficeThemeApi/';

        var service = {
            getConfig: getConfig,
            saveConfig: saveConfig,
            getThemes: getThemes,
            applyTheme: applyTheme,
            resetTheme: resetTheme
        };

        return service; 

        /////////
        function getConfig() {
            return $http.get(serviceRoot + 'GetConfig')
                .then(function (data) {
                    return data;
                });
        }

        function getThemes() {
            return $http.get(serviceRoot + 'GetThemes');
        }

        function applyTheme(alias) {
            return $http.put(serviceRoot + 'ApplyTheme/?alias=' + alias);
        }

        function resetTheme() {
            return $http.put(serviceRoot + 'ResetTheme/');
        }

        function saveConfig(config) {
            return $http.post(serviceRoot + "SaveConfig/", config);
        }

    }


    angular.module('umbraco.services')
        .factory('backofficeThemeService', themeService);
})();